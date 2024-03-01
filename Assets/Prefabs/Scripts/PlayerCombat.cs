using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
[SerializeField]
    public float attackRange = 0.2f; //feel free to change the attack range too, not sure how much range were giving him at the base state
    public LayerMask enemyLayers;
    
    public void Attack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
	    animator.SetBool("IsAttacking", true);
	    StartCoroutine(AttackDelay());
            
            Debug.Log("Attackstarted");
	    
        }
    }
    IEnumerator AttackDelay()
    {	 
	yield return new WaitForSeconds(0.35f);
	AttackMechanic();
	yield return new WaitForSeconds(0.35f);

	animator.SetBool("IsAttacking", false);
    }
    void AttackMechanic()
    {
	
        //we cna put an attack animation here later if we make one
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " was hit.");
        }

        //damaging enemies later we need to make the enemies first
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
