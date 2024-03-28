using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState() 
    {
        //jump code
    }
    public override void UpdateState() { }
    public override void ExitState() { }
    public override void FixedUpdateState() { }
    public override void CheckSwitchStates() { }
    public override void InitializeSubState() { }
}
