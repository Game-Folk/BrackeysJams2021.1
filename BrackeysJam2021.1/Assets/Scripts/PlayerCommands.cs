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
    
    public static event Action<Transform> OnPrisonersRecruitedAndRecalled;
    
    private static float _recruitDistance = 5f;
    private static List<GameObject> prisonersUnoccupied = new List<GameObject>();
    private static List<GameObject> prisonersOccupied = new List<GameObject>();

    // Unoccupies all prisoners under player's control (recruited)
    public static void AddPrisonerToPlayersControl(GameObject prisonerGO)
    {
        // if an occupied prisoner, remove it from occupied
        if(prisonersOccupied.Contains(prisonerGO)) prisonersOccupied.Remove(prisonerGO);

        // if not already in list, add it
        if(!prisonersUnoccupied.Contains(prisonerGO)) prisonersUnoccupied.Add(prisonerGO);
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
            OnPrisonersRecruitedAndRecalled?.Invoke(this.transform);

            // unoccupy every prisoner
            foreach(GameObject prisonerGO in prisonersOccupied)
            {
                prisonersUnoccupied.Add(prisonerGO);
            }
            prisonersOccupied.Clear();
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

    private void SendPrisoner(Transform targetPos)
    {
        print(prisonersUnoccupied.Count);
        // check if prisoners available
        if(prisonersUnoccupied.Count < 1)
        {
            Debug.Log("PlayerCommands: No prisoners available.");
            // TODO: display a message on screen? a noise?
            return;
        }

        // Select 1st prisoner from list of unoccupiedPrisoners
        GameObject prisonerToOccupy = prisonersUnoccupied[0];
        prisonersOccupied.Add(prisonerToOccupy);
        prisonersUnoccupied.Remove(prisonerToOccupy);

        prisonerToOccupy.GetComponent<PrisonerMovement>().SetDestination(targetPos);
    }
}
