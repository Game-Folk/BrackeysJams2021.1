using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerMovement : MonoBehaviour
{
    void Awake()
    {
        PlayerCommands.OnPrisonersRecalled += RecallPrisoner;
        Debug.Log("PrisonerMovement: enrolled in event OnPrisonersRecalled");
    }

    private void RecallPrisoner(Transform playerPos)
    {

    }
}
