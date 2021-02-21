using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGateInteractable : InteractableObject
{
    [SerializeField] private GameObject destinationTargetPrefab = null;

    private bool gateBroken = false;

    public void RemoveConnectionsForDeath()
    {
        if(gateBroken) return;
        gateBroken = true;

        // release monkeys to stand by mode
        foreach(Transform minion in currentMinionTransforms)
        {
            // create target for monkey
            GameObject target = Instantiate(destinationTargetPrefab, minion.position, Quaternion.identity);
            minion.GetComponent<Monkey>().SetStandByLocation(target.transform);
        }

        // remove this object from lion's checks
        playerCommands.RemoveInteractableObject(transform);
    }
}
