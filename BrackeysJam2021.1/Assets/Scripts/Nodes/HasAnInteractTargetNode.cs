public class HasAnInteractTargetNode : Node
{
    private Monkey monkey;

    public HasAnInteractTargetNode(Monkey monkey)
    {
        this.monkey = monkey;
    }

    public override NodeState Evaluate()
    {
        _nodeState = monkey.GetInteractTarget() != null ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}
