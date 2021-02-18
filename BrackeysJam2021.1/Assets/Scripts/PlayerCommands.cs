using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommands : MonoBehaviour
{
    [SerializeField] private GameObject destinationTargetPrefab = null;
    [SerializeField] private float recruitDistance = 5f;

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
    }
    
    private static float _recruitDistance = 5f;
    private List<Monkey> unrecruitedMonkeysList = new List<Monkey>();
    private List<Monkey> occupiedMonkeysList = new List<Monkey>();
    private List<Monkey> unoccupiedMonkeysList = new List<Monkey>();

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

    public static float GetRecruitDistance()
    {
        return _recruitDistance;
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
        else if (Input.GetMouseButtonDown(0)) // Send prisoner to location/object
        {

            Vector3 worldPoint = Input.mousePosition;
            worldPoint.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(worldPoint);
            mouseWorldPosition.z = 0f;
            
            // Create target
            GameObject target = Instantiate(destinationTargetPrefab, mouseWorldPosition, Quaternion.identity);

            SendPrisoner(target.transform);
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
    }

    private void SendPrisoner(Transform targetPos)
    {
        // check if prisoners available
        if(unoccupiedMonkeysList.Count < 1)
        {
            Debug.Log("PlayerCommands: No monkeys available.");
            // TODO: display a message on screen? a noise?
            return;
        }

        // Select 1st prisoner from list of unoccupiedPrisoners
        Monkey monkeyToOccupy = unoccupiedMonkeysList[0];
        occupiedMonkeysList.Add(monkeyToOccupy);
        unoccupiedMonkeysList.Remove(monkeyToOccupy);

        // monkeyToOccupy.SetRecalledStatus(false);
        // monkeyToOccupy.SetDestination(targetPos);
        monkeyToOccupy.SetStandByLocation(targetPos);
    }
}
