using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class BaseAIUnit : BaseUnit
{
    [SerializeField] protected float updateAITimesPerSeconds = 10;
    [SerializeField] protected float massToPushThrough = 10f;
    [SerializeField] protected float timeToBeHeavy = 5f;
    [SerializeField] protected float attackSearchRange = 5f;

    private AIDestinationSetter aIDestinationSetter;
    private Rigidbody2D rgbd;
    protected BaseUnit attackTarget = null;
    protected Transform standByLocation = null;

    protected Node topNode;

    public override void Awake()
    {
        base.Awake();

        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        rgbd = GetComponent<Rigidbody2D>();
    }

    public override void Start()
    {
        base.Start();

        ConstructBehaviorTree();
        StartCoroutine(EvaluateBehaviorTree());
        StartCoroutine(Attack());
    }

    public override void Update()
    {
        base.Update();
    }
    
    public virtual void ConstructBehaviorTree()
    {

    }

    IEnumerator EvaluateBehaviorTree()
    {
        while(true)
        {
            // just wait again, if no topNode
            if(topNode == null) 
            {
                yield return new WaitForSeconds(1/updateAITimesPerSeconds);
                continue;
            }

            topNode.Evaluate();
            yield return new WaitForSeconds(1/updateAITimesPerSeconds);
        }
    }

    public virtual IEnumerator Attack()
    {
        while(true)
        {
            // just wait again, if no target
            if(attackTarget == null) 
            {
                yield return new WaitForSeconds(1/attacksPerSecond);
                continue;
            }

            if(Vector2.Distance(attackTarget.transform.position, transform.position) < attackRange)
            {
                attackTarget.AddCurrentHealth(-attack);
            }
            yield return new WaitForSeconds(1/attacksPerSecond);
        }
        
    }

    // the idea here, is that we reset the actions from the behavior tree
    // except for the action we're choosing
    public virtual void ResetActionFlags(string nameOfAction)
    {
        if(!nameOfAction.Equals("Attacking"))
        {
            attackTarget = null;
        }
        if(!nameOfAction.Equals("Standing By"))
        {
            standByLocation = null;
        }
    }
    
    public bool SetDestination(Transform dest)
    {
        // Set its mass so it can push through
        StartCoroutine(ResetMass(rgbd.mass, timeToBeHeavy));
        rgbd.mass = massToPushThrough;

        aIDestinationSetter.target = dest;

        return true;
    }
    private IEnumerator ResetMass(float mass_OG, float timeToBeHeavy)
    {
        yield return new WaitForSeconds(timeToBeHeavy);
        rgbd.mass = mass_OG; // reset the mass
    }

    public BaseUnit GetAttackTarget()
    {
        return attackTarget;
    }
    public void SetAttackTarget(BaseUnit newAttackTarget)
    {
        attackTarget = newAttackTarget;
        ResetActionFlags("Attacking");
    }

    public Transform GetStandByLocation()
    {
        return standByLocation;
    }
    public void SetStandByLocation(Transform newStandByLocation)
    {
        standByLocation = newStandByLocation;
        print("set " + standByLocation);
        ResetActionFlags("Standing By");
    }
}
