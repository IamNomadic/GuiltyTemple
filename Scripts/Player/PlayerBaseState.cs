using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);

    public abstract void ExitState(PlayerStateManager player);
    public abstract void FixedUpdateState(PlayerStateManager player);
    
}
