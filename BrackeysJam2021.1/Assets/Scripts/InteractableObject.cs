using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected int minionsRequired = 1;
    [SerializeField] private float minionMoveDistance = 1f;

    protected PlayerCommands playerCommands = null;
    protected int currentMinions = 0;
    protected List<Transform> currentMinionTransforms = new List<Transform>();

    public virtual void Awake()
    {
        
    }

    public virtual void Start()
    {
        playerCommands = PlayerCommands.instance;
        playerCommands.AddInteractableObject(transform); // register with the player
    }

    public virtual void Update()
    {
        foreach(Transform minion in currentMinionTransforms)
        {
            float distance = Vector2.Distance(minion.position, transform.position);
            if(distance < minionMoveDistance)
            {
                currentMinions++;
            }
        }

        CheckIfDoThing();
        currentMinions = 0;
    }

    public virtual void CheckIfDoThing()
    {

    }

    public void AddMinion(Transform t){
        currentMinionTransforms.Add(t);
    }

    public void RemoveMinion(Transform t){
        currentMinionTransforms.Remove(t);
    }

    public void RemoveAllMinions(){
        currentMinionTransforms.Clear();
    }
}
