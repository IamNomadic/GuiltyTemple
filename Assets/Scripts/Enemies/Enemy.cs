using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //note to self; enemy class > enemy types will be children with variables like damage, health, range, etc changed
    #region player stuff
    [SerializeField]                    //get our player stuff first
    protected Object playerObject;        // who is your daddy
    [SerializeField]
    protected PlayerHealth playerHealth;  // what does he do
    #endregion

    #region Enemy properties
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected int enemyMaxHealth;
    protected int enemyCurrentHealth;
    protected BoxCollider2D enemyCollider; //gotta check if any of these need to be culled when I'm done here
    protected Rigidbody2D rb;
    [SerializeField]
    public bool inKnockback = false;
    [SerializeField]
    protected float knockbackTimer = 0.4f; //how long are we in knockback mode
    protected bool shoved = false; //has the knockback effect occurred yet
    protected bool movingRight = true;
    protected float horizontal;
    protected float vertical;
    [SerializeField]
    protected bool isFlier = false; //is this a flying enemy
    [SerializeField]
    protected bool isMeleeAttacker = false; //whack
    [SerializeField]
    protected bool isRangedAttacker = false; //do we have a ranged attack
    [SerializeField]
    protected EnemyProjectile projectile; //what's our projectile if we're a ranged attacker?
    [SerializeField]
    protected GameObject projectileSpawn; //where to create the projectile
    [SerializeField]
    protected float throwForce = 1;
    #endregion

    #region EnemyAttack
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float attackWindup; //time between starting the attack and enabling the hitbox/projectile
    [SerializeField]
    protected float attackCooldown; //total time that the attack will take
    protected float cooldownTimer = Mathf.Infinity;
    [SerializeField]
    protected float moveSpeed = 1;
    protected bool canAttack = true;
    [SerializeField]
    protected float attackRange = 0.3f; //start swinging at this range
    #endregion

    #region Patrol Stuff
    [SerializeField]
    protected GameObject patrolA; //waypoint 1
    [SerializeField]
    protected GameObject patrolB; //waypoint 2
    [SerializeField]
    protected float patrolSpeed = 1;
    protected Transform currentPatrolTarget;
    #endregion

    #region EnemyDetection
    [SerializeField]
    protected float detectionRange = 1.0f;
    [SerializeField]
    protected float colliderDistance;
    [SerializeField]
    protected LayerMask playerLayer;
    #endregion

    //state machine setup
    public enum Behavior
    {
        idle, patrol, pursuit, attack, knockback, die, dying
    }
    #region state machine stuff
    public Behavior currentState = Behavior.idle;
    protected Behavior nextState = Behavior.idle;
    protected Transform playerTarget;      //do we have a target?
    protected bool isStateFinished = true; //can we proceed to the next state now?
    protected bool interruptState = false; //can we interrupt this state?
    protected bool patrolRoute = false;    
    #endregion



    protected void Start()
    {
        enemyCurrentHealth = enemyMaxHealth; //initialize at full health
        enemyCollider = GetComponent<BoxCollider2D>(); //grab our box and stuff    
        rb = GetComponent<Rigidbody2D>();
        animator.Play("Idle");
        if (isFlier)
        {
            rb.gravityScale = 0;
        }
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
    protected void Update()
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

            switch (currentState)
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
                    ///interruptState = false;
                    //isStateFinished = false;
                    break;
                case Behavior.dying:
                    Debug.Log("Death is upon us");
                    break;
            }
        


    }

    protected void CheckFacing()
    {       
        if (!movingRight && rb.velocity.x > 0f && !inKnockback)
        {
            Flip();
        }
        else if (movingRight && rb.velocity.x < 0f && !inKnockback)
        {
            Flip();
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

    protected void Idle()
    {

        interruptState = true;

        if (!isFlier && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) //set our idle, flying or not
        {
            animator.Play("Idle");
        }
        if (isFlier && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
        {
            animator.Play("Fly");
        }


        if (PlayerInSight()) //scanning area
        {
            playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;
            nextState = Behavior.pursuit;
        }

        else if (patrolRoute) // patrol instead of idling if we have a route
        {
            nextState = Behavior.patrol;
        }      
    }

    protected void Patrol() // simple point A to point B, back and forth
    {
        interruptState = true;

        Vector2 point = currentPatrolTarget.position - transform.position;

        if (isFlier)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
            {
                animator.Play("Fly");
            }
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                animator.Play("Walk");
            }
        }

        if (PlayerInSight())
        {
            playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;
            nextState = Behavior.pursuit;
        }
        CheckFacing();
        if (currentPatrolTarget == patrolB.transform)
            {
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            }
        else
            {
                rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
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

    protected virtual void Pursuit()
    {
        //monk time
        //
        CheckFacing();
        if (Mathf.Abs(playerTarget.position.x - rb.transform.position.x) < attackRange && canAttack)    //swing within out attack range
        {
            nextState = Behavior.attack;
        }
        interruptState = true;
        playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;

        if (!isFlier) //walk it off
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                animator.Play("Walk");
            }
            if (playerTarget.position.x > rb.transform.position.x) // check which way we're going
            {
                //Debug.Log("right");
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else
            {
                //Debug.Log("left");
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
        }
        if (isFlier) //wingaling movement
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
            {
                animator.Play("Fly");
            }
            Vector2 flyDirection = (playerObject.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;
            rb.velocity = flyDirection * moveSpeed;

        }

        if (!PlayerInSightChasing())
        {
            nextState = Behavior.idle;      //go back to patrolling or idling if we lose sight of the player
        }


    }

    protected virtual void Attack()
    {
        interruptState = true;
        if (canAttack && isMeleeAttacker)
        {
            MeleeAttack();
        }
        else if (canAttack && isRangedAttacker)
        {
            RangedAttack();
        }

    }
    protected virtual void MeleeAttack()

    {
        
        {
            CheckFacing(); //check facing once at the start of the attack
            canAttack = false;
            //SpriteRenderer hbSprite = GetComponentInChildren<SpriteRenderer>();
            //hbSprite.color = Color.red;    //we're turning red
            StartCoroutine(AttackTimer());
            animator.Play("Attack");
            IEnumerator AttackTimer()
            {
                //Debug.Log("attack timer started");
                yield return new WaitForSeconds(attackCooldown); //transmit damage to the player here when the player state machine is done
                                                                 //hbSprite.color = Color.white;
                if (nextState != Behavior.die)
                {
                    nextState = Behavior.idle;
                    isStateFinished = true;
                    canAttack = true;
                }

            }
        }
    }
    protected virtual void RangedAttack()
    {
        
        {
            //find the bearing from the enemy to the player
            //produce the projectile and propel it towards them
            CheckFacing(); //check facing once at the start of the attack
            canAttack = false;
            Vector2 throwDirection = (playerObject.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized;
            StartCoroutine(RangedAttackWindup());
            StartCoroutine(RangedAttackTimer());
            animator.Play("RangedAttack");
            IEnumerator RangedAttackWindup()
            {
                Transform newProjLocation = projectileSpawn.transform;
                yield return new WaitForSeconds(attackWindup);
                EnemyProjectile newProjectile = Instantiate(projectile, newProjLocation.position, new Quaternion(0, 0, 0, 0));
                newProjectile.BeThrown(throwDirection, throwForce, damage);
            }
            IEnumerator RangedAttackTimer()
            {
                yield return new WaitForSeconds(attackCooldown);
                if (nextState != Behavior.die)
                {
                    nextState = Behavior.idle;
                    isStateFinished = true;
                    canAttack = true;
                }

            }
        }
    }


    protected void Knockback()
    {
        //Debug.Log("knockback entered");
        //ouch
        animator.Play("Struck");
        interruptState = false;               
        Vector2 knockbackDirection = (playerObject.GetComponent<Rigidbody2D>().transform.position - transform.position).normalized; //figure out where to push them back
        //rb.AddForce(knockbackDirection * -2f);   
        if (!shoved)  //are we actually moving?
        {
            shoved = true;
            rb.velocity = knockbackDirection * -3;
            StartCoroutine(KnockbackTimer());
        }

        IEnumerator KnockbackTimer()
        {
            yield return new WaitForSeconds(knockbackTimer);
            isStateFinished = true;
            shoved = false;
            inKnockback = false;
            if (enemyCurrentHealth > 0) // death check after the knockback
            {
                nextState = Behavior.idle;
            }
            else                        //otherwise just hit idle so another behavior can take over
            {
                nextState = Behavior.die;
            }


        }
    }

    protected void Die()
    {
        nextState = Behavior.dying;
        interruptState = true;
        isStateFinished = true;
        Debug.Log("We are still dying");
        animator.Play("Die");          //away with us
        //interruptState = false;
        rb.gravityScale = 1;
        StartCoroutine(Death());
        /*  I feel like we want to just make the collider ignore the player rather than destroying anything
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        foreach (Collider2D Collider2D in gameObject.GetComponents<Collider2D>())
        {
            Destroy(Collider2D);
        }
        */
        //if we have a dead sprite for these enemies we can just disable here instead to leave the body
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(1);
        float alphaBlendTime = 100f;
        for (int i = 0; i < alphaBlendTime; i++)
        {
            float alpha = 1.0f - (float)i / alphaBlendTime;
            SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
            spriteRender.material.color = new Color(spriteRender.material.color.r, spriteRender.material.color.g, spriteRender.material.color.b, alpha);
            Debug.LogFormat("I is {0}", i);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("We made it");
        Destroy(this.gameObject); 
    }
    protected bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.CircleCast(enemyCollider.bounds.center, detectionRange, Vector2.right, 0, playerLayer); //constant vigilance
        return hit.collider != null;
    }

    protected bool PlayerInSightChasing()       //double the sight range so enemies will chase a little further than they can spot you
    {
        RaycastHit2D hit = Physics2D.CircleCast(enemyCollider.bounds.center, detectionRange * 2, Vector2.right, 0, playerLayer); //constant vigilance
        return hit.collider != null;
    }
    /*
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyCollider.bounds.center, detectionRange);
    }
     */

    public void TakeDamage(int damage)        
    {

        //Debug.Log("damage");
        if (!inKnockback)
        {
            inKnockback = true;                 //make sure we're not dealing damage multiple times with one swing
            enemyCurrentHealth -= damage;       //deal the damage
            nextState = Behavior.knockback;  //change the state
        }
    }
    protected  void Flip() //snip
    {
        //Debug.Log("flip");
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

}

