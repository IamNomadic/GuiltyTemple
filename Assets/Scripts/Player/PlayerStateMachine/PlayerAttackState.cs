using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState() 
    {
        Attack();
    }
    public override void UpdateState() 
    {
        //this is update per frame
    }
    public override void ExitState() 
    {
        _context.isAttacking = false;
    }
    public override void FixedUpdateState() 
    {
        //update per fixed frame
    }
    public override void CheckSwitchStates() 
    { 
        //changes from this state to another when a condition is met
    }
    public override void InitializeSubState() 
    { 

    }
    IEnumerator AttackWaitTime()
    {
        yield return new WaitForSeconds(0.55f);
        _context.isAttacking = false;
    }
    void Attack()
    {
        
        _context.isAttacking = true;
        
        _context.animator.Play("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_context.attackPoint.position, _context.attackRange, _context.enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " was hit.");

            enemy.GetComponent<Enemy>().TakeDamage(_context.playerATKDamage);
        }
        AttackWaitTime();
    }
   
}
