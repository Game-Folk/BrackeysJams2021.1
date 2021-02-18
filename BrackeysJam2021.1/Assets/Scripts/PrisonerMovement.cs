using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PrisonerMovement : MonoBehaviour
{
    [SerializeField] private float massToPushThrough = 10f;
    [SerializeField] private float timeToBeHeavy = 5f;
    [SerializeField] private Color recruitColor;

    private AIDestinationSetter aIDestinationSetter;
    private Rigidbody2D rgbd;
    private bool recruited = false;

    void Awake()
    {
        // PlayerCommands.OnPrisonersRecruitedAndRecalled += RecruitAndRecallPrisoner;

        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        rgbd = GetComponent<Rigidbody2D>();
    }

    public void SetDestination(Transform dest)
    {
        // SetDestination only if already recruited
        if(!recruited) return;

        // Set its mass so it can push through
        StartCoroutine(ResetMass(rgbd.mass, timeToBeHeavy));
        rgbd.mass = massToPushThrough;

        aIDestinationSetter.target = dest;
    }

    public bool GetRecruitedStatus()
    {
        return recruited;
    }

    // private void RecruitAndRecallPrisoner(Transform playerPos)
    // {
    //     // recruit
    //     if(!recruited)
    //     {
    //         // check recruitDistance
    //         float distance = Vector3.Distance(this.transform.position, playerPos.position);
    //         if(distance < PlayerCommands.GetRecruitDistance())
    //         {
    //             // recruit!
    //             recruited = true;
    //             PlayerCommands.AddPrisonerToPlayersControl(this.gameObject);
    //             // set recruited color
    //             this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = recruitColor;
    //         }
    //     }

    //     // recall only if already recruited
    //     if(!recruited) return;
    //     aIDestinationSetter.target = playerPos;
    // }

    private IEnumerator ResetMass(float mass_OG, float timeToBeHeavy)
    {
        yield return new WaitForSeconds(timeToBeHeavy);
        rgbd.mass = mass_OG; // reset the mass
    }
}
