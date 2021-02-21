using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zookeeper : BaseAIUnit
{
    [SerializeField] private float chanceToSpawnKey = 0.5f; // percentage
    [SerializeField] private GameObject keyPrefab = null;

    private UnitManager unitManager;
    private bool isZookeeperDead = false;

    public override void Start()
    {
        unitManager = UnitManager.instance;
        unitManager.AddHumanToAttackableList(transform); // register with unitmanager

        base.Start();
    }

    public override void ConstructBehaviorTree()
    {
        base.ConstructBehaviorTree();

        IfAnyAttackTargetInRangeSetClosestNode ifAnyAttackTargetInRangeSetClosestNode =
            new IfAnyAttackTargetInRangeSetClosestNode(attackSearchRange, unitManager.GetAttackableAnimalsList(), this);
        SetFollowTargetNode setFollowTargetNode = new SetFollowTargetNode(this);
        HasALocationTargetNode hasALocationTargetNode = new HasALocationTargetNode(this);

        Sequence attackingSequence = new Sequence(new List<Node> { ifAnyAttackTargetInRangeSetClosestNode, setFollowTargetNode });
        Sequence standingBySequence = new Sequence(new List<Node> { hasALocationTargetNode, setFollowTargetNode });

        topNode = new Selector(new List<Node> { attackingSequence, standingBySequence });
    }

    public override void Die(float timeUntilDestroy)
    {
        base.Die(timeUntilDestroy);

        if(isZookeeperDead) return; // only die once
        isZookeeperDead = true;
        
        unitManager.RemoveHumanFromAttackableList(this.transform);

        // spawn a key by chance
        int num = Random.Range(1, 101);
        if(num <= chanceToSpawnKey*100) // if num less than 50%
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
        }
    }
}
