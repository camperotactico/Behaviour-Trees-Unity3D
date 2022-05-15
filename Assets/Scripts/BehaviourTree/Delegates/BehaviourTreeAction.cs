namespace BehaviourTrees
{
    /// <summary>
    /// Delegate to be used by a behaviour tree action.
    /// </summary>
    /// <returns> A <c>BehaviourTreeState</c> value. </returns>
    public delegate BehaviourTreeState BehaviourTreeAction();
}