
public abstract class PlayerBaseState
{
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _factory;
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _context = currentContext;
    }
    public abstract void EnterState();
    public abstract void UpdateState();

    public abstract void ExitState();
    public abstract void FixedUpdateState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    void UpdateStates() { }
    protected void SwitchState(PlayerBaseState newState)
    { 
        //current state exits
        ExitState();

        //enter new state
        newState.EnterState();

        //switch current state context
        _context.CurrentState = newState;
    }
    protected void SetSuperState() { }
    protected void SetSubState() { }
}
