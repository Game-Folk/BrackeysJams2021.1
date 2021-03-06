﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BigKey : InteractableObject
{
    [SerializeField] private float distToUnlock = 0.5f;
    [SerializeField] private float deathTime = 0.5f;
    [SerializeField] private GameObject destinationTargetPrefab = null;
    [SerializeField] private UnlockableGateInteractable unlockableGateInteractable = null;

    private AIDestinationSetter aIDestinationSetter;
    private AIPath aIPath;
    private float speedOG = 1;
    private bool keyRedeemed = false;

    public override void Awake()
    {
        base.Awake();
        
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();
    }

    public override void Start()
    {
        base.Start();

        if(unlockableGateInteractable == null) Debug.Log("BigKey: did u forget to set my gate to unlock");

        aIDestinationSetter.target = unlockableGateInteractable.transform; // target is the gate

        speedOG = aIPath.maxSpeed;
    }

    public override void Update()
    {
        base.Update();

        if(keyRedeemed) return; // stop as soon as we've redeemed key

        // if close enough to monkey spawner, unlock cage, spawn a monkey
        float distance = Vector2.Distance(unlockableGateInteractable.transform.position, transform.position);
        if(distance < distToUnlock)
        {
            keyRedeemed = true;

            unlockableGateInteractable.OpenGate();
            
            // release monkeys to stand by mode
            foreach(Transform minion in currentMinionTransforms)
            {
                // create target for monkey
                GameObject target = Instantiate(destinationTargetPrefab, minion.position, Quaternion.identity);
                minion.GetComponent<Monkey>().SetStandByLocation(target.transform);
            }

            // remove key from lion's checks
            playerCommands.RemoveInteractableObject(transform);

            // key death animation?

            // destroy key
            Destroy(this.gameObject, deathTime);
        }
    }

    public override void CheckIfDoThing()
    {
        base.CheckIfDoThing();
        
        if(currentMinions >= minionsRequired)
        {
            aIPath.maxSpeed = speedOG; // go
        }
        else if(currentMinions < minionsRequired)
        {
            aIPath.maxSpeed = 0; // stop
        }
    }
}
