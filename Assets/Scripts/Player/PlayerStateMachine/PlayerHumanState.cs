using UnityEngine;

public class PlayerHumanState : PlayerBaseState
{
    public PlayerHumanState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("I AM HUMAN");
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override void FixedUpdateState()
    {
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
    }
}