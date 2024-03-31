using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public PlayerInputs playerInputs;
    public Transform attackPoint;
    public PlayerHealth ph;
    [SerializeField]
    public float attackRange = 0.2f;
    public LayerMask enemyLayers;
    public bool isAttacking;
    int playerATKDamage = 1;


    public void Attack()
    {
        if (!ph.dead)
        {
            isAttacking = true;
        }
        animator.Play("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " was hit.");
            enemy.SendMessage("TakeDamage", playerATKDamage);
            //enemy.GetComponent<Enemy>().TakeDamage(playerATKDamage);
        }
        
        IEnumerator AttackWaitTime()
        {
            yield return new WaitForSeconds(0.55f);
            isAttacking = false;
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
