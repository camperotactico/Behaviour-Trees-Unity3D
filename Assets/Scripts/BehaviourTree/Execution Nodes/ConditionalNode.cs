namespace BehaviourTrees
{
    internal class ConditionalNode : IExecutionNode
    {
        private BehaviourTreeConditional conditional;

        internal ConditionalNode(BehaviourTreeConditional newConditional)
        {
            conditional = newConditional;
        }

        public BehaviourTreeState Tick()
        {
            return conditional.Invoke() ? BehaviourTreeState.SUCCESS : BehaviourTreeState.FAILURE;
        }
    }
}