namespace BehaviourTrees
{
    internal class ParallelNode : AbstractControlFlowNode
    {
        private int threshold;
        int successes;
        int failures;

        internal ParallelNode(int successThreshold)
        {
            threshold = successThreshold;
        }

        public override BehaviourTreeState Tick()
        {
            successes = 0;
            failures = 0;
            foreach (BehaviourTree child in GetChildren())
            {
                BehaviourTreeState state = child.Tick();
                if (state.Equals(BehaviourTreeState.SUCCESS))
                {
                    successes++;
                }
                if (state.Equals(BehaviourTreeState.FAILURE))
                {
                    failures++;
                }
            }

            if (successes >= threshold)
            {
                return BehaviourTreeState.SUCCESS;
            }
            if (failures > GetChildren().Count - threshold)
            {
                return BehaviourTreeState.FAILURE;
            }

            return BehaviourTreeState.RUNNING;

        }

        public int GetSuccessThreshold()
        {
            return threshold;
        }
    }
}
