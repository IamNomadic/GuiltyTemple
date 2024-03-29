using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //note to self; enemy class > enemy types will be children with variables like damage, health, range, etc changed
    #region player stuff
    [SerializeField]                    //get our player stuff first
    private Object playerObject;        // who is your daddy
    [SerializeField]
    private PlayerHealth playerHealth;  // what does he do
    #endregion

    #region Enemy properties
    [SerializeField]
    public int enemyMaxHealth;
    int enemyCurrentHealth;
    private BoxCollider2D enemyCollider; //gotta check if any of these need to be culled when I'm done here
    private Rigidbody2D rb;
    private bool inKnockback = false;
    [SerializeField]
    private float knockbackTimer = 0.4f; //how long are we in knockback mode
    bool shoved = false; //has the knockback effect occurred yet
    #endregion

    #region EnemyAttack
    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField]
    private float moveSpeed = 1;
    #endregion

    #region Patrol Stuff
    [SerializeField]
    private GameObject patrolA; //waypoint 1
    [SerializeField]
    private GameObject patrolB; //waypoint 2
    [SerializeField]
    private float patrolSpeed = 1;
    private Transform currentPatrolTarget;
    #endregion

    #region EnemyDetection
    [SerializeField]
    private float detectionRange = 1.0f;
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private LayerMask playerLayer;
    #endregion

    //state machine setup
    public enum Behavior
    {
        idle, patrol, pursuit, attack, knockback, die
    }
    #region state machine stuff
    public Behavior currentState = Behavior.idle;
    private Behavior nextState = Behavior.idle;
    private Transform playerTarget;      //do we have a target
    private bool isStateFinished = true; //all done
    private bool interruptState = false; //can we switch to a different state?
    private bool patrolRoute = false;
    #endregion



    private void Start()
    {
        enemyCurrentHealth = enemyMaxHealth; //initialize at full health
        enemyCollider = GetComponent<BoxCollider2D>(); //grab our box and stuff    
        rb = GetComponent<Rigidbody2D>();
        Initialize();

        if (patrolA && patrolB != null)                //set our patrol destination if we have one
        {
            patrolRoute = true;
            currentPatrolTarget = patrolB.transform;
            nextState = Behavior.patrol;   //get to it immediately if we have waypoints
        }
        else
        {
            nextState = Behavior.idle;
        }

    }
    public void Update()
    {
        /*
        if (PlayerInSight())
        {
            {
                Debug.Log("spotted");
            }
        }
        */

        if (currentState != nextState && (isStateFinished || interruptState))
        {
            currentState = nextState;
            isStateFinished = false;
        }
            switch(currentState)
            {
                case Behavior.idle:
                    Idle();
                    break;
                case Behavior.patrol:
                    Patrol();
                    break;
                case Behavior.pursuit:
                    Pursuit();
                    break;
                case Behavior.attack:
                    Attack();
                    break;
                case Behavior.knockback:
                    Knockback();
                    break;
                case Behavior.die:
                    Die();
                    break;
            }        
    }

    // commenting all of this out while I transplant these behaviours into a state machine
    /*
    private void FixedUpdate()
    {
        cooldownTimer += Time.deltaTime;
        if( PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
              
            DamagePlayer();
            Debug.Log("you were hit");
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

    //player damages enemy - damage and knockback logic
    public void TakeDamage(int damage)
    {          
        enemyCurrentHealth -= damage;          
        Vector2 knockbackDirection = playerObject.GetComponent<Rigidbody2D>().transform.position - transform.position; //figure out where to push them back         
        rb.AddForce(knockbackDirection * 15f);
        if (enemyCurrentHealth <= 0)
        {
            nextState = Behavior.die;
        }

    }
    */


    private void Initialize()   // just in case child enemies need to do anything on Start() without overriding the work we do in this script
    {                           // don't even know if we'll need this but it was a problem I had in the physicsville assignment that I now have a solution to

    }

    private void Idle()
    {

        interruptState = true;
        
        if(PlayerInSight())
        {
            playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;
            currentState = Behavior.pursuit;
        }

        else if (patrolRoute)
        {
            currentState = Behavior.patrol;
        }

        // play idle animation and wait if the player isn't nearby and we have no assigned patrol route        
    }

    private void Patrol() // simple point A to point B, back and forth
    {
        interruptState = true;

        Vector2 point = currentPatrolTarget.position - transform.position;
        
        if (PlayerInSight())
        {
            playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;
            nextState = Behavior.pursuit;
        }
        
        if (currentPatrolTarget == patrolB.transform)
            {
                rb.velocity = new Vector2(patrolSpeed, 0);
            }
        else
            {
                rb.velocity = new Vector2(-patrolSpeed, 0);
            }
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < .2f && currentPatrolTarget == patrolA.transform)
            {
                currentPatrolTarget = patrolB.transform;
            }
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < .2f && currentPatrolTarget == patrolB.transform)
            {
                currentPatrolTarget = patrolA.transform;
            }

    }

    private void Pursuit()
    {
        //we're chasing the player now
        //move to their location, check if we're in attacking range and our attack is off cooldown
        interruptState = true;
        playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;
        //this is going to be different per enemy so idk if I should put anything in this script
    }

    private void Attack()
    {
        //thwack
        //pow
        //this is going to be different per enemy so idk if I should put anything in this script
    }
    private void Knockback()
    {
        //Debug.Log("knockback entered");
        //ouch
        interruptState = false;               
        Vector2 knockbackDirection = (playerObject.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized; //figure out where to push them back
        //rb.AddForce(knockbackDirection * -2f);   
        StartCoroutine(KnockbackTimer());
        if (!shoved)                //are we actually moving?
        {
            shoved = true;
            rb.velocity = knockbackDirection * -3;
        }

        IEnumerator KnockbackTimer()
        {

            yield return new WaitForSeconds(knockbackTimer);
            shoved = false;
            inKnockback = false;
            isStateFinished = true;
            if (enemyCurrentHealth <= 0) // death check after the knockback
            {
                nextState = Behavior.die;
            }
            else                        //otherwise just hit idle so another behavior can take over
            {
                nextState = Behavior.idle;
            }
        }
    }

    private void Die()
    {
        //enemy death animation
        //Debug.Log("AUUUUUUUUUGUHHHGUHUHHH");
        Destroy(gameObject); //away with us
        //if we have a dead sprite for these enemies we can just disable here instead to leave the body
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.CircleCast(enemyCollider.bounds.center, detectionRange, Vector2.right, 0, playerLayer); //constant vigilance
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyCollider.bounds.center, detectionRange);
    }
        
    public void TakeDamage(int damage)        
    {
        Debug.Log("damage");
        if (!inKnockback)
        {
            inKnockback = true;                 //make sure we're not dealing damage multiple times with one swing
            enemyCurrentHealth -= damage;       //deal the damage
            nextState = Behavior.knockback;  //change the state
        }
    }

}

