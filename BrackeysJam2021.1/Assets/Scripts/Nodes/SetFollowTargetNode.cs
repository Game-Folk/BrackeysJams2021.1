using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFollowTargetNode : Node
{
    private BaseAIUnit baseAIUnit;

    public SetFollowTargetNode(BaseAIUnit baseAIUnit)
    {
        this.baseAIUnit = baseAIUnit;
    }

    public override NodeState Evaluate()
    {
        // NOTE: should always return true... as I don't do any error checking
        // NOTE 2: target doesn't get unset... but is also set couple times a second
        // Set the destination to the target's transform
        _nodeState = baseAIUnit.SetDestination(null) ? NodeState.RUNNING : NodeState.FAILURE;
        return _nodeState;
    }
}
