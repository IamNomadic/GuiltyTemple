using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public PlayerInputs playerInputs;
    public Transform attackPoint;
    [SerializeField]
    public float attackRange = 0.2f;
    public LayerMask enemyLayers;
    
    int playerATKDamage = 1;


    public void Attack()
    {
        animator.Play("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " was hit.");
            
            enemy.GetComponent<Enemy>().TakeDamage(playerATKDamage);
        }
        
        IEnumerator AttackWaitTime()
        {
            yield return new WaitForSeconds(0.55f);
            
        }
        StartCoroutine(AttackWaitTime());
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
