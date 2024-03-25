using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    public PlayerHumanState humanState = new PlayerHumanState();
    public PlayerVampireState vampireState = new PlayerVampireState();
    public PlayerWolfState PlayerWolfState = new PlayerWolfState();




    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = humanState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    private void OnDisable()
    {
        currentState.ExitState(this);
    }

}
