using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Monkey : BaseAIUnit
{
    [SerializeField] private Color recruitColor;

    private EnemySpawner enemySpawner = null;
    private PlayerCommands playerCommands = null;
    private bool recruited = false;
    private bool recalled = false;
    private InteractableObject interactTarget = null;
    private Transform interactTargetTransform = null;

    public override void Awake()
    {
        base.Awake();
    }
    
    public override void Start()
    {
        enemySpawner = EnemySpawner.instance;
        playerCommands = PlayerCommands.instance;

        playerCommands.AddToUnrecruitedList(this);

        base.Start();
    }
    
    public override void ConstructBehaviorTree()
    {
        base.ConstructBehaviorTree();

        IsRecruitedNode isRecruitedNode = new IsRecruitedNode(this);
        HasBeenRecalledNode hasBeenRecalledNode = new HasBeenRecalledNode(this);
        SetFollowTargetNode setFollowPlayerNode = new SetFollowTargetNode(this, PlayerCommands.instance.transform);
        HasAnInteractTargetNode hasAnInteractTargetNode = new HasAnInteractTargetNode(this);
        SetFollowTargetNode setFollowInteractableNode = new SetFollowTargetNode(this, interactTargetTransform);
        IfAnyAttackTargetInRangeSetClosestNode ifAnyAttackTargetInRangeSetClosestNode = 
            new IfAnyAttackTargetInRangeSetClosestNode(attackSearchRange, enemySpawner.GetEnemyTransformsList(), this);
        Transform attackTargetTransform = null;
        if(attackTarget != null) attackTargetTransform = attackTarget.transform;
        SetFollowTargetNode setFollowAttackTargetNode = new SetFollowTargetNode(this, attackTargetTransform);
        HasALocationTargetNode hasALocationTargetNode = new HasALocationTargetNode(this);
        print("Monkey: " + standByLocation);
        SetFollowTargetNode setFollowStandByTargetNode = new SetFollowTargetNode(this, standByLocation);

        Sequence followingSequence = new Sequence(new List<Node> { hasBeenRecalledNode, setFollowPlayerNode });
        Sequence interactingSequence = new Sequence(new List<Node> { hasAnInteractTargetNode, setFollowInteractableNode });
        Sequence attackingSequence = new Sequence(new List<Node> { ifAnyAttackTargetInRangeSetClosestNode, setFollowAttackTargetNode });
        Sequence standingBySequence = new Sequence(new List<Node> { hasALocationTargetNode, setFollowStandByTargetNode });

        Selector pickActionSelector = new Selector(new List<Node> { followingSequence, interactingSequence, 
                                                            attackingSequence, standingBySequence });

        Sequence pickRecruitedActionSequence = new Sequence(new List<Node> { isRecruitedNode, pickActionSelector });

        topNode = new Selector(new List<Node> { pickRecruitedActionSequence });
    }

    public override void ResetActionFlags(string nameOfAction)
    {
        base.ResetActionFlags(nameOfAction);

        if(nameOfAction != "Following")
        {
            recalled = false;
        }
        if(nameOfAction != "Interacting")
        {
            interactTarget = null;
            interactTargetTransform = null;
        }
    }
    
    public bool GetRecruitedStatus()
    {
        return recruited;
    }
    public void SetRecruitedStatus(bool b)
    {
        recruited = b;
        
        if(recruited)
        {
            // set recruited color
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = recruitColor;
        }
    }
    
    public bool GetRecalledStatus()
    {
        return recalled;
    }
    public void SetRecalledStatus(bool b)
    {
        recalled = b;
        ResetActionFlags("Following");
    }

    public InteractableObject GetInteractTarget()
    {
        return interactTarget;
    }
    public void SetInteractTarget(InteractableObject newInteractTarget)
    {
        interactTarget = newInteractTarget;
        ResetActionFlags("Interacting");
    }
    public Transform GetInteractTargetTransform()
    {
        return interactTargetTransform;
    }
    public void SetInteractTargetTransform(Transform newInteractTargetTransform)
    {
        interactTargetTransform = newInteractTargetTransform;
    }
}
