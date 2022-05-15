namespace BehaviourTrees
{
    internal class ActionNode : IExecutionNode
    {
        private BehaviourTreeAction action;

        internal ActionNode(BehaviourTreeAction newAction)
        {
            action = newAction;
        }

        public BehaviourTreeState Tick()
        {
            return action.Invoke();
        }
    }
}