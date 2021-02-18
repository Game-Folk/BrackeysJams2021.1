public class IsRecruitedNode : Node
{
    private Monkey monkey;

    public IsRecruitedNode(Monkey monkey)
    {
        this.monkey = monkey;
    }

    public override NodeState Evaluate()
    {
        _nodeState = monkey.GetRecruitedStatus() ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}
