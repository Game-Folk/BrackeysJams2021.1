public class HasBeenRecalledNode : Node
{
    private Monkey monkey;

    public HasBeenRecalledNode(Monkey monkey)
    {
        this.monkey = monkey;
    }

    public override NodeState Evaluate()
    {
        _nodeState = monkey.GetRecalledStatus() ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}
