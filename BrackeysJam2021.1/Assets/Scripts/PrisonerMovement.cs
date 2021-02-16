using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PrisonerMovement : MonoBehaviour
{
    private AIDestinationSetter aIDestinationSetter;

    void Awake()
    {
        PlayerCommands.OnPrisonersRecalled += RecallPrisoner;
        PlayerCommands.AddPrisonerToPlayersControl(this.gameObject);

        aIDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    public void SetDestination(Transform dest)
    {
        aIDestinationSetter.target = dest;
    }

    private void RecallPrisoner(Transform playerPos)
    {
        SetDestination(playerPos);
    }
}
