using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfAnyAttackTargetInRangeSetClosestNode : Node
{
    private float range;
    private List<Transform> targets;
    private BaseAIUnit origin;

    public IfAnyAttackTargetInRangeSetClosestNode(float range, List<Transform> targets, BaseAIUnit origin)
    {
        this.range = range;
        this.targets = targets;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        // Find closest target
        Transform closestTarget = null; 
        float minDistance = Mathf.Infinity;
        foreach(Transform t in targets)
        {
            float distance = Vector2.Distance(t.position, origin.transform.position);
            if(distance < minDistance)
            {
                closestTarget = t;
                minDistance = distance;
            }
        }
        // If in range and target exists
        if(minDistance < range && closestTarget != null)
        {
            BaseUnit targetUnit = closestTarget.GetComponent<BaseUnit>();
            origin.SetAttackTarget(targetUnit);
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }
        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }
}
