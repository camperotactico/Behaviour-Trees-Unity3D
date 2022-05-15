namespace BehaviourTrees
{
    /// <summary>
    /// The execution state of a behaviour tree.
    /// </summary>
    public enum BehaviourTreeState
    {
        /// <summary>
        /// Successful execution state.
        /// </summary>
        SUCCESS,
        /// <summary>
        /// Running execution state.
        /// </summary>
        RUNNING,
        /// <summary>
        /// Failure execution state.
        /// </summary>
        FAILURE
    }
}