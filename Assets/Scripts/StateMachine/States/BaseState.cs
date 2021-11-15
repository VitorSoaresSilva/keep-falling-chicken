public abstract class BaseState
{
    public StateMachine owner;
    /// <summary>
    /// Method called to prepare state to operate - like Unity's Start()
    /// </summary>
    public virtual void PrepareState(){}
    
    /// <summary>
    /// Method called to update state on every frame - like Unity's Update()
    /// </summary>
    public virtual void UpdateState(){}
    
    /// <summary>
    /// Method called to destroy state
    /// </summary>
    public virtual void DestroyState(){}
    
}
