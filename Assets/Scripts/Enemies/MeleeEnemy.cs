using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D enemyCollider;
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    public int enemyMaxHealth;
    int enemyCurrentHealth;

    #region EnemyDetection
    [SerializeField]
    private float detectionRange;
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private LayerMask playerLayer;
    #endregion

    #region EnemyAttack

    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    
    #endregion

    private void Start()
    {
        enemyCurrentHealth = 1;
    }
    private void FixedUpdate()
    {
        cooldownTimer += Time.deltaTime;
        if( PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            DamagePlayer();
            //add attack animation and adjust timing so damage function is done in time with attack
        }
    }
    //damaging player
    private void DamagePlayer()
    {
        if(PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
    //player damages enemy
    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;

        //damage taking aniamtion

        if(enemyCurrentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        //enemy death aniamtion
        Debug.Log("AUUUUUUUUUGUHHHGUHUHHH");
        Destroy(gameObject);

    }
    //the code here is so ugly im sorry
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(enemyCollider.bounds.center + colliderDistance * detectionRange * transform.localScale.x * transform.right, 
            new Vector3(enemyCollider.bounds.size.x * detectionRange, enemyCollider.bounds.size.y, enemyCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyCollider.bounds.center + colliderDistance * detectionRange * transform.localScale.x * transform.right,
            new Vector3(enemyCollider.bounds.size.x * detectionRange, enemyCollider.bounds.size.y, enemyCollider.bounds.size.z));
    }
}
