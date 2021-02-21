using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftableGateInteractable : InteractableObject
{
    [SerializeField] private Sprite openGateSprite = null;
    [SerializeField] private List<SpriteRenderer> gateSprites = new List<SpriteRenderer>();
    [SerializeField] private GameObject destinationTargetPrefab = null;

    private bool gateLifted = false;

    public override void CheckIfDoThing()
    {   
        base.CheckIfDoThing();

        if(currentMinions >= minionsRequired)
        {
            OpenGate();
        }
    }

    private void OpenGate()
    {
        if(gateLifted) return;
        gateLifted = true;

        // open gate sprites
        foreach(SpriteRenderer s in gateSprites)
        {
            s.sprite = openGateSprite;
        }

        // disable collider
        GetComponent<Collider2D>().enabled = false;
            
        // release monkeys to stand by mode
        foreach(Transform minion in currentMinionTransforms)
        {
            // create target for monkey
            GameObject target = Instantiate(destinationTargetPrefab, minion.position, Quaternion.identity);
            minion.GetComponent<Monkey>().SetStandByLocation(target.transform);
        }

        // remove key from lion's checks
        playerCommands.RemoveInteractableObject(transform);

        // recalculate A* paths
        AstarPath.active.Scan();

        // disable follower counter
        followerCountText.gameObject.SetActive(false);
    }
}
