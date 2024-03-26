using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState() { }
    public override void UpdateState() { }
    public override void ExitState() { }
    public override void FixedUpdateState() { }
    public override void CheckSwitchStates() 
    { 
     //default state
     //switch to wlak when walking
     //jump state when jumping
     //attacking state when attacking
    }
    public override void InitializeSubState() { }
}
