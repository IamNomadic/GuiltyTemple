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
    private InputAction attack;
    int playerATKDamage = 1;

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
            
            enemy.GetComponent<MeleeEnemy>().TakeDamage(playerATKDamage);
            //change later to include all enemy types
            //note to self(damien): try making a base enemy class and have enemy types as children
        }
        
        IEnumerator AttackWaitTime()
        {
            yield return new WaitForSeconds(0.55f);
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
