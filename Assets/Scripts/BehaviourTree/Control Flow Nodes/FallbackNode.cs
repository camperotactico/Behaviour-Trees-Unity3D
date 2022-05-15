namespace BehaviourTrees
{
    internal class FallbackNode : AbstractControlFlowNode
    {
        public override BehaviourTreeState Tick()
        {
            foreach (BehaviourTree child in GetChildren())
            {
                BehaviourTreeState state = child.Tick();
                if (!state.Equals(BehaviourTreeState.FAILURE))
                {
                    return state;
                }
            }
            return BehaviourTreeState.FAILURE;
        }
    }
}
