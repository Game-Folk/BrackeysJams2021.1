using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Monkey : BaseAIUnit
{
    [SerializeField] private Color recruitColor;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private UnitManager unitManager = null;
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
        unitManager = UnitManager.instance;
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
        SetFollowTargetNode setFollowTargetNode = new SetFollowTargetNode(this);
        HasAnInteractTargetNode hasAnInteractTargetNode = new HasAnInteractTargetNode(this);
        IfAnyAttackTargetInRangeSetClosestNode ifAnyAttackTargetInRangeSetClosestNode = 
            new IfAnyAttackTargetInRangeSetClosestNode(attackSearchRange, unitManager.GetAttackableHumansList(), this);
        Transform attackTargetTransform = null;
        if(attackTarget != null) attackTargetTransform = attackTarget.transform;
        HasALocationTargetNode hasALocationTargetNode = new HasALocationTargetNode(this);

        Sequence followingSequence = new Sequence(new List<Node> { hasBeenRecalledNode, setFollowTargetNode });
        Sequence interactingSequence = new Sequence(new List<Node> { hasAnInteractTargetNode, setFollowTargetNode });
        Sequence attackingSequence = new Sequence(new List<Node> { ifAnyAttackTargetInRangeSetClosestNode, setFollowTargetNode });
        Sequence standingBySequence = new Sequence(new List<Node> { hasALocationTargetNode, setFollowTargetNode });

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

    public override void Die(float timeUntilDestroy)
    {
        unitManager.RemoveAnimalFromAttackableList(this.transform);

        base.Die(timeUntilDestroy);
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
            spriteRenderer.color = recruitColor;

            // make attackable
            unitManager.AddAnimalToAttackableList(transform);
        }
    }
    
    public bool GetRecalledStatus()
    {
        return recalled;
    }
    public void SetRecalledStatus(bool b)
    {
        recalled = b;
        targetTransform = PlayerCommands.instance.transform;
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
        targetTransform = newInteractTargetTransform;
    }
}
