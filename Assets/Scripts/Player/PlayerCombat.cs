using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;


public class PlayerCombat : MonoBehaviour
{
    public static event Action OnPlayerDamaged;

    public Animator animator;
    public PlayerInputs playerInputs;
    public Transform attackPoint;
    public GameObject bigAttackLoc;
    PlayerHealth pH;
    PlayerMovement pM;
    [SerializeField]
    public float attackRange = 0.2f;
    public LayerMask enemyLayers;
    public bool isAttacking;
    int playerATKDamage = 1;

    private void Start()
    {
        pH = GetComponent<PlayerHealth>();
        pM = GetComponent<PlayerMovement>();
        bigAttackLoc.SetActive(false);
    }
    public void Attack()
    {
        if (!pH.dead && !isAttacking)
        {
            isAttacking = true;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            if (pM.WTransformed)
            {
                bigAttackLoc.SetActive(true);
            }
            
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log(enemy.name + " was hit.");
                enemy.SendMessage("TakeDamage", playerATKDamage);
                //enemy.GetComponent<Enemy>().TakeDamage(playerATKDamage);
                if (pM.VTransformed &&  pH.currentHealth < pH.maxHealth)
                {
                    pH.currentHealth += 1;
                    OnPlayerDamaged?.Invoke();
                }
            }
            StartCoroutine(AttackWaitTime());
        }
        
        
        
        IEnumerator AttackWaitTime()
        {
            yield return new WaitForSeconds(0.55f);
            isAttacking = false;
            bigAttackLoc.SetActive(false);
        }
        
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
