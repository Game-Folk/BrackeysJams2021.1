using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFollowTargetNode : Node
{
    private Monkey monkey;
    private Transform target;

    public SetFollowTargetNode(Monkey monkey, Transform target)
    {
        this.monkey = monkey;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        // NOTE: should always return true... as I don't do any error checking
        // NOTE 2: target doesn't get unset... but is also set couple times a second
        // Set the destination to the target's transform
        _nodeState = monkey.SetDestination(target) ? NodeState.RUNNING : NodeState.FAILURE;
        Debug.Log("set dest " + target);
        Debug.Log(target);
        return _nodeState;
    }
}
