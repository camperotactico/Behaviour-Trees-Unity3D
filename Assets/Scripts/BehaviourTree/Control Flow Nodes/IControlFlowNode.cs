
using System.Collections.Generic;

namespace BehaviourTrees
{
    internal interface IControlFlowNode : BehaviourTree
    {
        void AddChild(BehaviourTree behaviourTree);

        List<BehaviourTree> GetChildren();

    }
}