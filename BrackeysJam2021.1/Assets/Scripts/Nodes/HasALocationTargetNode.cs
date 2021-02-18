public class HasALocationTargetNode : Node
{
    private BaseAIUnit aIUnit;

    public HasALocationTargetNode(BaseAIUnit aIUnit)
    {
        this.aIUnit = aIUnit;
    }

    public override NodeState Evaluate()
    {
        _nodeState = aIUnit.GetStandByLocation() != null ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}
