using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public GameObject bigAttackLoc;

    [SerializeField] public float attackRange = 0.2f;

    public LayerMask enemyLayers;
    public bool isAttacking;
    private PlayerHealth pH;
    private readonly int playerATKDamage = 1;
    public PlayerInputs playerInputs;
    private PlayerMovement pM;

    private void Start()
    {
        pH = GetComponent<PlayerHealth>();
        pM = GetComponent<PlayerMovement>();
        bigAttackLoc.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public static event Action OnPlayerDamaged;

    public void Attack()
    {
        if (!pH.dead && !isAttacking)
        {
            isAttacking = true;

            var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            if (pM.WTransformed) bigAttackLoc.SetActive(true);

            foreach (var enemy in hitEnemies)
            {
                Debug.Log(enemy.name + " was hit.");
                enemy.SendMessage("TakeDamage", playerATKDamage);
                //enemy.GetComponent<Enemy>().TakeDamage(playerATKDamage);
                if (pM.VTransformed && pH.currentHealth < pH.maxHealth)
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
}