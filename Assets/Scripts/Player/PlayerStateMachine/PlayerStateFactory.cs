using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
    PlayerStateMachine context;
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
    }
    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(context, this);
    }
    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(context, this);
    }
    public PlayerBaseState Attack()
    { 
        return new PlayerAttackState(context, this);
    }
    public PlayerBaseState Dash()
    { 
        return new PlayerDashState(context, this);
    }
    public PlayerBaseState HumanS()
    {
        return new PlayerHumanState(context, this);
    }
    public PlayerBaseState WolfS()
    {
        return new PlayerWolfState(context, this);
    }
    public PlayerBaseState VampireS()
    {
        return new PlayerVampireState(context, this);
    }
}
