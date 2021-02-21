using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCommands : MonoBehaviour
{
    [SerializeField] private GameObject destinationTargetPrefab = null;
    [SerializeField] private float timeToDestroyUnassignedTarget = 3f;
    [SerializeField] private float recruitDistance = 5f;
    [SerializeField] private TMP_Text followerCountText = null;
    [SerializeField] protected Animator animator = null;
    [SerializeField] private AudioSource recallAudio = null;

    public static PlayerCommands instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one PlayerCommands in scene!");
            return;
        }
        instance = this;

        _recruitDistance = recruitDistance;
        lion = GetComponent<Lion>();
    }
    
    private static float _recruitDistance = 5f;
    private List<Monkey> unrecruitedMonkeysList = new List<Monkey>();
    private List<Monkey> occupiedMonkeysList = new List<Monkey>();
    private List<Monkey> unoccupiedMonkeysList = new List<Monkey>();
    private List<Transform> interactableObjects = new List<Transform>();
    private List<InteractableObject> currentInteractedObjects = new List<InteractableObject>();

    private Lion lion = null;

    public void AddToUnrecruitedList(Monkey monkey)
    {
        unrecruitedMonkeysList.Add(monkey);
    }

    // Unoccupies all monkeys under player's control (has been recruited)
    public void AddMonkeyToPlayersControl(Monkey monkey)
    {
        // if monkey was occupied, unoccupy it
        if(occupiedMonkeysList.Contains(monkey)) occupiedMonkeysList.Remove(monkey);

        // if monkey is not already in the unoccupied list, add it
        if(!unoccupiedMonkeysList.Contains(monkey)) unoccupiedMonkeysList.Add(monkey);
    }

    public void RemoveMonkeyFromPlayer(Monkey monkey)
    {
        if(unrecruitedMonkeysList.Contains(monkey)) unrecruitedMonkeysList.Remove(monkey);
        if(occupiedMonkeysList.Contains(monkey)) occupiedMonkeysList.Remove(monkey);
        if(unoccupiedMonkeysList.Contains(monkey)) unoccupiedMonkeysList.Remove(monkey);
    }

    public static float GetRecruitDistance()
    {
        return _recruitDistance;
    }

    public void AddInteractableObject(Transform t)
    {
        interactableObjects.Add(t);
    }
    public void RemoveInteractableObject(Transform t)
    {
        interactableObjects.Remove(t);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) // "Recall" and/or "Recruit" Prisoners
        {
            // TODO: instantiate a circle of recruit radius

            // Recruit, then recall all already recruited
            RecruitAndRecall();
        }
        else if(Input.GetMouseButtonDown(0)) // left click to stomp
        {
            lion.StompAttack();
        }
        else if (Input.GetMouseButtonDown(1)) // right click Send prisoner to location/object
        {
            Vector3 worldPoint = Input.mousePosition;
            worldPoint.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(worldPoint);
            mouseWorldPosition.z = 0f;

            // Create target
            GameObject target = Instantiate(destinationTargetPrefab, mouseWorldPosition, Quaternion.identity);

            bool prisonerHasTarget = SendPrisoner(target.transform);
            if(!prisonerHasTarget) Destroy(target, timeToDestroyUnassignedTarget);
        }
    }

    private void RecruitAndRecall()
    {
        List<Monkey> recruitedMonkeys = new List<Monkey>();
        foreach(Monkey m in unrecruitedMonkeysList)
        {
            // if not yet recruited
            if(!m.GetRecruitedStatus())
            {
                // check recruit distance
                float distance = Vector2.Distance(m.transform.position, transform.position);
                if(distance < GetRecruitDistance())
                {
                    // recruit!
                    m.SetRecruitedStatus(true);
                    AddMonkeyToPlayersControl(m);
                    recruitedMonkeys.Add(m);
                }
            }
        }
        foreach(Monkey m in recruitedMonkeys)
        {
            unrecruitedMonkeysList.Remove(m);
        }     

        Recall();
    }

    private void Recall()
    {
        List<Monkey> newOccupiedMonkeysList = new List<Monkey>();
        foreach(Monkey m in occupiedMonkeysList)
        {
            newOccupiedMonkeysList.Add(m);
            m.SetRecalledStatus(true);
        }
        foreach(Monkey m in newOccupiedMonkeysList)
        {
            AddMonkeyToPlayersControl(m);
        }
        foreach(Monkey m in unoccupiedMonkeysList)
        {
            m.SetRecalledStatus(true);
        }
        UpdateFollowerCounts();

        // reduce the count on each of the objects we've sent them to
        foreach(InteractableObject io in currentInteractedObjects)
        {
            io.RemoveAllMinions();
        }
        
        // Play recall animation
        animator.SetTrigger("Recall");

        // Play recall sound
        recallAudio.Play();
    }

    // true if success
    // false if failure
    private bool SendPrisoner(Transform targetPos)
    {
        // check if prisoners available
        if(unoccupiedMonkeysList.Count < 1)
        {
            Debug.Log("PlayerCommands: No monkeys available.");
            // TODO: display a message on screen? a noise?
            return false;
        }

        // Select 1st prisoner from list of unoccupiedPrisoners
        Monkey monkeyToOccupy = unoccupiedMonkeysList[0];
        occupiedMonkeysList.Add(monkeyToOccupy);
        unoccupiedMonkeysList.Remove(monkeyToOccupy);
        UpdateFollowerCounts();
        
        // Play Send animation
        animator.SetTrigger("Send");

        // Interactable Object ------------------
        foreach(Transform interactableObject in interactableObjects)
        {
            float distance = Vector2.Distance(interactableObject.position, targetPos.position);
            if(distance < 0.5f)
            {
                // interO
                InteractableObject interO = interactableObject.GetComponent<InteractableObject>();
                interO.AddMinion(monkeyToOccupy.transform);
                currentInteractedObjects.Add(interO); // keep track of where your minions are

                // send minion
                monkeyToOccupy.SetInteractTarget(interO);
                monkeyToOccupy.SetInteractTargetTransform(interactableObject);

                return false; // returning false so this script takes care of deleting target
            }
        }
        
        // Location ----------------------------------
        monkeyToOccupy.SetStandByLocation(targetPos);
        return true;
    }

    private void UpdateFollowerCounts()
    {
        int unoccupied = unoccupiedMonkeysList.Count;
        int total = occupiedMonkeysList.Count + unoccupied;
        followerCountText.text = unoccupied + "/" + total;
    }
}
