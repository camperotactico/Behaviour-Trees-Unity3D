using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    /// <summary>
    /// Interface for a behaviour tree. Given the modular nature of BTs, each node should implement this interface as they also are a BT themselves.
    /// </summary>
    public interface BehaviourTree
    {
        /// <summary>
        /// Ticks this behaviour tree recursively.
        /// </summary>
        /// <returns>The <c>BehaviourTreeState</c> of this behaviour tree.</returns>
        BehaviourTreeState Tick();

        /// <summary>
        /// Builder class to construct a <c>BehaviourTree</c>.
        /// </summary>
        public sealed class Builder
        {
            private BehaviourTree root;
            private Stack<IControlFlowNode> openedControlFlowNodes = new Stack<IControlFlowNode>();

            /// <summary>
            /// Opens a fallback control flow node.
            /// <para>
            /// Propagates the tick signal received to its children one at a time  upon return.
            /// Returns the state of the first child to return either SUCCESS or RUNNING.
            /// Returns FAILURE only if all children return FAILURE.
            /// </para>
            /// </summary>
            /// <returns>the <c>Builder</c> itself.</returns>
            public Builder OpenFallback()
            {
                AddControlFlowNode(new FallbackNode());
                return this;
            }

            /// <summary>
            /// Opens a sequence control flow node. 
            /// <para>
            /// Propagates the tick signal received to its children one at a time upon return.
            /// Returns the state of the first child to return either FAILURE or RUNNING.
            /// Returns SUCCESS only if all children return SUCCESS.
            /// </para>
            /// </summary>
            /// <returns>the <c>Builder</c> itself.</returns>
            public Builder OpenSequence()
            {
                AddControlFlowNode(new SequenceNode());
                return this;
            }
            /// <summary>
            /// Opens a parallel control flow node.
            /// <para>
            /// Propagates the tick signal received to its children at the same time.
            /// Takes a success threshold as input.Returns SUCCESS if the number of children
            /// that return SUCCESS is equal or greater than the specified threshold.Returns 
            /// FAILURE if the number of children that return FAILURE is greater than the total
            /// number of children minus the threshold specified. Otherwise it returns RUNNING.
            /// </para>
            /// </summary>
            /// <param name="successThreshold"> the number of child nodes that need to success for this node to be success.</param>
            /// <returns>the <c>Builder</c> itself.</returns>
            /// <exception cref="UnityException">
            /// Throws if the provied threshold is less than 1.
            /// </exception>
            public Builder OpenParallel(int successThreshold)
            {
                if (successThreshold < 1)
                {
                    throw new UnityException("Threshold must be greater than 0.");
                }
                AddControlFlowNode(new ParallelNode(successThreshold));
                return this;
            }

            /// <summary>
            ///  Closes the last control flow node opened.
            /// </summary>
            /// <returns>the <c>Builder</c> itself.</returns>
            /// <exception cref="UnityException">
            /// Throws if there are no open control flow nodes or it does not have any children.
            /// </exception>
            public Builder Close()
            {
                if (openedControlFlowNodes.Count < 1)
                {
                    throw new UnityException("There are no control flow nodes open.");
                }
                else if (openedControlFlowNodes.Peek()
                        .GetChildren()
                        .Count < 1)
                {
                    throw new UnityException("The control flow node has no children.");
                }

                IControlFlowNode closedControlFlowNode = openedControlFlowNodes.Pop();
                if (closedControlFlowNode is ParallelNode)
                {
                    ValidateParallelNode((ParallelNode)closedControlFlowNode);
                }


                return this;
            }

            /// <summary>
            /// Adds an action execution node to the behaviour tree. Requires a delegate that returns a behaviour state.
            /// </summary>
            /// <param name="action">
            /// The <c>BehaviourTreeAction</c> delegate to invoke.
            /// </param>
            /// <returns>the <c>Builder</c> itself.</returns>
            public Builder Action(BehaviourTreeAction action)
            {
                AddBehaviourTree(new ActionNode(action));
                return this;
            }

            /// <summary>
            /// Adds a conditional execution node to the behaviour tree.  Requires a delegate that returns a boolean.
            /// </summary>
            /// <param name="conditional">
            /// The <c>BehaviourTreeConditional</c> delegate to invoke.
            /// </param>
            /// <returns>the <c>Builder</c> itself.</returns>
            public Builder Conditional(BehaviourTreeConditional conditional)
            {
                AddBehaviourTree(new ConditionalNode(conditional));
                return this;
            }

            /// <summary>
            /// Builds a behaviour tree using the specified structure. 
            /// </summary>
            /// <returns>
            /// A <c>BehaviourTree</c> with the specified structure.
            /// </returns>
            /// <exception cref="UnityException">
            /// Throws if there are any opened control flow nodes or the tree is empty.
            /// </exception>
            public BehaviourTree Build()
            {
                if (openedControlFlowNodes.Count > 0)
                {
                    throw new UnityException(
                            openedControlFlowNodes.Count + " Control flow node(s) were not properly closed.");
                }
                else if (root == null)
                {
                    throw new UnityException("Trying to build an empty behaviour tree.");
                }
                return root;
            }


            private void SetRoot(BehaviourTree behaviourTree)
            {
                if (root == null)
                {
                    root = behaviourTree;
                }
                else
                {
                    throw new UnityException("It is not possible to add another node at root level.");
                }
            }

            private void AddBehaviourTree(BehaviourTree behaviourTree)
            {
                if (openedControlFlowNodes.Count < 1)
                {
                    SetRoot(behaviourTree);
                }
                else
                {
                    openedControlFlowNodes.Peek()
                            .AddChild(behaviourTree);
                }
            }

            private void AddControlFlowNode(IControlFlowNode controlFlowNode)
            {
                AddBehaviourTree(controlFlowNode);
                openedControlFlowNodes.Push(controlFlowNode);
            }

            private void ValidateParallelNode(ParallelNode parallelNode)
            {

                int threshold = parallelNode.GetSuccessThreshold();
                if (threshold > parallelNode.GetChildren().Count)
                {
                    throw new UnityException("Threshold (" + threshold
                            + ")must be equal or smaller than the children size (" + parallelNode.GetChildren()
                                    .Count
                            + ")");
                }

            }

        }


    }

}