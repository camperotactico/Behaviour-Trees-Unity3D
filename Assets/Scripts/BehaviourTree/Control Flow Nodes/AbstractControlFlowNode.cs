using System.Collections.Generic;

namespace BehaviourTrees
{
    /// <summary>
    /// This abstract control flow node should be implemented in order to construct custom Decorator control nodes.
    /// </summary>
    internal abstract class AbstractControlFlowNode : IControlFlowNode
    {
        private List<BehaviourTree> children = new List<BehaviourTree>();

        public void AddChild(BehaviourTree behaviourTree)
        {
            children.Add(behaviourTree);
        }

        public List<BehaviourTree> GetChildren()
        {
            return children;
        }

        public abstract BehaviourTreeState Tick();
    }
}