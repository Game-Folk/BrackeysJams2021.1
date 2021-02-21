using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableGateInteractable : MonoBehaviour
{
    [SerializeField] private Sprite openGateSprite = null;
    [SerializeField] private List<SpriteRenderer> gateSprites = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> lockSprites = new List<SpriteRenderer>();

    private bool gateLifted = false;

    public void OpenGate()
    {
        if(gateLifted) return;
        gateLifted = true;

        // open gate sprites
        foreach(SpriteRenderer s in gateSprites)
        {
            s.sprite = openGateSprite;
        }
        // disable lock sprites
        foreach(SpriteRenderer s in lockSprites)
        {
            s.enabled = false;
        }

        // disable collider
        GetComponent<Collider2D>().enabled = false;

        // recalculate A* paths
        AstarPath.active.Scan();

        // bring up victory
        GetComponent<EnableCanvas>().EnableThisCanvas();
    }
}
