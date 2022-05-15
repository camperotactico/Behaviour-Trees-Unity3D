namespace BehaviourTrees
{
    internal class SequenceNode : AbstractControlFlowNode
    {
        public override BehaviourTreeState Tick()
        {
            foreach (BehaviourTree child in GetChildren())
            {
                BehaviourTreeState state = child.Tick();
                if (!state.Equals(BehaviourTreeState.SUCCESS))
                {
                    return state;
                }
            }
            return BehaviourTreeState.SUCCESS;
        }
    }
}