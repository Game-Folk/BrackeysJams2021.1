using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected bool interactable = true;
    [SerializeField] protected int minionsRequired = 1;
    [SerializeField] private float minionMoveDistance = 1f;
    [SerializeField] protected TMP_Text followerCountText = null;

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
        if(!interactable) return;

        int minionsInRangeRequired = 0;
        foreach(Transform minion in currentMinionTransforms)
        {
            float distance = Vector2.Distance(minion.position, transform.position);
            if(distance < minionMoveDistance)
            {
                minionsInRangeRequired++;
            }
        }

        currentMinions = minionsInRangeRequired;
        UpdateFollowerCounts();
        CheckIfDoThing();
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

    private void UpdateFollowerCounts()
    {
        int unoccupied = currentMinions;
        int total = minionsRequired;
        if(total == 0) // any minions allowed
        {
            followerCountText.text = unoccupied.ToString();
        }
        else
        {
            followerCountText.text = unoccupied + "/" + total;
        }
    }
}
