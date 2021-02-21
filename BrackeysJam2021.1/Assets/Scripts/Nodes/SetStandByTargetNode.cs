using UnityEngine;

// Note: not sure why I wrote this... must be getting tired...

public class SetStandByTargetNode : Node
{
    private BaseAIUnit aIUnit;
    private Transform standByLocation;

    public SetStandByTargetNode(BaseAIUnit aIUnit, Transform standByLocation)
    {
        this.aIUnit = aIUnit;
        this.standByLocation = standByLocation;
    }

    public override NodeState Evaluate()
    {
        _nodeState = aIUnit.SetDestination(standByLocation) ? NodeState.RUNNING : NodeState.FAILURE;
        aIUnit.SetStandByLocation(standByLocation);
        return _nodeState;
    }
}
