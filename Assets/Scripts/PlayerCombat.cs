using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    [SerializeField]
    public float attackRange = 0.2f;
    public LayerMask enemyLayers;
    public PlayerInputs playerInputs;
    private InputAction attack;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }
    private void OnEnable()
    {
        attack = playerInputs.Player.Attack;
        attack.Enable();
    }
    private void OnDisable()
    {
        attack.Disable();
    }
    private void FixedUpdate()
    {
        if (attack.triggered)
        {
            Debug.Log("attack attempted");
            Attack(); 
        }
    }

    private void Attack()
    {
        animator.SetBool("IsAttacking", true);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " was hit.");
            //damage enemies here
        }
        
        IEnumerator AttackWaitTime()
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("IsAttacking", false);
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
