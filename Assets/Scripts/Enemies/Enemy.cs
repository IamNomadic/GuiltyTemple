using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    //note to self; enemy class > enemy types will be children with variables like damage, health, range, etc changed

    #region player stuff

    [SerializeField] //get our player stuff first
    protected Object playerObject; // who is your daddy
    [SerializeField] protected PlayerHealth playerHealth; // what does he do

    #endregion

    #region Enemy properties

    [SerializeField] protected Animator animator;
    [SerializeField] protected int enemyMaxHealth;

    [SerializeField] protected int enemyCurrentHealth;
    protected BoxCollider2D enemyCollider; //gotta check if any of these need to be culled when I'm done here
    protected Rigidbody2D rb;

    [SerializeField] public bool inKnockback;

    [SerializeField] protected float knockbackTimer = 0.4f; //how long are we in knockback mode

    protected bool shoved; //has the knockback effect occurred yet
    protected bool movingRight = true;
    protected float horizontal;
    protected float vertical;

    [SerializeField] protected bool isFlier; //is this a flying enemy
    [SerializeField] protected bool isMeleeAttacker; //whack
    [SerializeField] protected bool isRangedAttacker; //do we have a ranged attack
    [SerializeField] protected EnemyProjectile projectile; //what's our projectile if we're a ranged attacker?
    [SerializeField] protected GameObject projectileSpawn; //where to create the projectile
    [SerializeField] protected bool multiProjectile; // does this actually fire three projectiles???
    [SerializeField] protected GameObject projectileSpawn2;
    [SerializeField] protected GameObject projectileSpawn3;
    [SerializeField] protected float throwForce = 1;
    [SerializeField] protected bool armoredMeleeAttacks;

    #endregion

    #region EnemyAttack

    [SerializeField] public int damage;
    [SerializeField] protected float attackWindup; //time between starting the attack and enabling the hitbox/projectile
    [SerializeField] protected float attackCooldown; //total time that the attack will take
    protected float cooldownTimer = Mathf.Infinity;
    [SerializeField] protected float moveSpeed = 1;
    protected bool canAttack = true;
    [SerializeField] protected float attackRange = 0.3f; //start swinging at this range
    [SerializeField] protected float rangedAttackRange = 3.0f;
    protected bool hitboxActive; //this is going to link to the animator trigger to link the active hitbox to the animation
    protected bool attackLanded; //check if this attack has connected so it doesn't stack

    #endregion

    #region Patrol Stuff

    [SerializeField] protected GameObject patrolA; //waypoint 1
    [SerializeField] protected GameObject patrolB; //waypoint 2
    [SerializeField] protected float patrolSpeed = 1;
    protected Transform currentPatrolTarget;

    #endregion

    #region EnemyDetection

    [SerializeField] protected float detectionRange = 1.0f;
    [SerializeField] protected float colliderDistance;
    [SerializeField] protected LayerMask playerLayer;

    #endregion

    #region state machine stuff

    public Behavior currentState = Behavior.idle;
    protected Behavior nextState = Behavior.idle;
    protected Transform playerTarget; //do we have a target?
    protected bool isStateFinished = true; //can we proceed to the next state now?
    protected bool interruptState; //can we interrupt this state?
    protected bool patrolRoute;

    #endregion
    //state machine setup
    public enum Behavior
    {
        idle,
        patrol,
        pursuit,
        attack,
        knockback,
        die,
        dying
    }


    protected void Start()
    {
        enemyCurrentHealth = enemyMaxHealth; //initialize at full health
        enemyCollider = GetComponent<BoxCollider2D>(); //grab our box and stuff    
        rb = GetComponent<Rigidbody2D>();
        animator.Play("Idle");
        if (isFlier) rb.gravityScale = 0;
        if (patrolA && patrolB != null) //set our patrol destination if we have one
        {
            patrolRoute = true;
            currentPatrolTarget = patrolB.transform;
            nextState = Behavior.patrol; //get to it immediately if we have waypoints
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
        if (hitboxActive && !attackLanded)
        {

        }
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
                interruptState = false;
                isStateFinished = false;
                break;
        }
    }

    protected void CheckFacing()
    {
        if (!movingRight && rb.velocity.x > 0f && !inKnockback)
            Flip();
        else if (movingRight && rb.velocity.x < 0f && !inKnockback) Flip();
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
            animator.Play("Idle");
        if (isFlier && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fly")) animator.Play("Fly");


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
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fly")) animator.Play("Fly");
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) animator.Play("Walk");
        }

        if (PlayerInSight())
        {
            playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;
            nextState = Behavior.pursuit;
        }

        CheckFacing();
        if (currentPatrolTarget == patrolB.transform)
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < .2f &&
            currentPatrolTarget == patrolA.transform) currentPatrolTarget = patrolB.transform;
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < .2f &&
            currentPatrolTarget == patrolB.transform) currentPatrolTarget = patrolA.transform;
    }

    protected virtual void Pursuit()
    {
        //monk time
        //
        CheckFacing();
        if (Mathf.Abs(playerTarget.position.x - rb.transform.position.x) < attackRange && canAttack) //swing within out attack range
            nextState = Behavior.attack;
        interruptState = true;
        playerTarget = playerObject.GetComponent<Rigidbody2D>().transform;

        if (!isFlier) //walk it off
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) animator.Play("Walk");
            if (playerTarget.position.x > rb.transform.position.x) // check which way we're going
                //Debug.Log("right");
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            else
                //Debug.Log("left");
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        if (isFlier) //wingaling movement
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fly")) animator.Play("Fly");
            Vector2 flyDirection = (playerObject.GetComponent<Rigidbody2D>().transform.position - transform.position)
                .normalized;
            rb.velocity = flyDirection * moveSpeed;
        }

        if (!PlayerInSightChasing())
            nextState = Behavior.idle; //go back to patrolling or idling if we lose sight of the player
    }

    protected virtual void Attack()
    {
        if (enemyCurrentHealth <= 0)
        {
            StopAllCoroutines();
            nextState = Behavior.die;
            isStateFinished = true;
        }
        if (armoredMeleeAttacks && enemyCurrentHealth > 0)
        {
            interruptState = false;
        }
        else
        {
            interruptState = true;
        }

        if (canAttack && isMeleeAttacker && isRangedAttacker)
        {
            if (Random.Range(1, 100) > 50)
            {
                MeleeAttack();
            }
            else
            {
                RangedAttack();
            }
        }
        else if (canAttack && isMeleeAttacker)
            MeleeAttack();
        else if (canAttack && isRangedAttacker) 
            RangedAttack();
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
                yield return
                    new WaitForSeconds(attackCooldown); //transmit damage to the player here when the player state machine is done
                //hbSprite.color = Color.white;
                if (nextState != Behavior.die && nextState != Behavior.dying)
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
            Vector2 throwDirection = (playerObject.GetComponent<Rigidbody2D>().transform.position - transform.position)
                .normalized;
            StartCoroutine(RangedAttackWindup());
            StartCoroutine(RangedAttackTimer());
            animator.Play("RangedAttack");

            IEnumerator RangedAttackWindup()
            {
                var newProjLocation = projectileSpawn.transform;
                yield return new WaitForSeconds(attackWindup);
                var newProjectile = Instantiate(projectile, newProjLocation.position, new Quaternion(0, 0, 0, 0));
                newProjectile.BeThrown(throwDirection, throwForce, damage);

                if (multiProjectile) //boss projectile stuff
                {
                    var newProjLocation2 = projectileSpawn2.transform;
                    var newProjLocation3 = projectileSpawn3.transform;
                    var newProjectile2 = Instantiate(projectile, newProjLocation2.position, new Quaternion(0, 0, 0, 0));
                    newProjectile2.BeThrown(throwDirection, throwForce, damage);
                    var newProjectile3 = Instantiate(projectile, newProjLocation3.position, new Quaternion(0, 0, 0, 0));
                    newProjectile3.BeThrown(throwDirection, throwForce, damage);
                }
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
        if (!shoved) //are we actually moving?
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
                nextState = Behavior.idle;
            else //otherwise just hit idle so another behavior can take over
                nextState = Behavior.die;
        }
    }

    protected void Die()
    {
        StopAllCoroutines();
        nextState = Behavior.dying;
        interruptState = false;
        //Debug.Log("We are still dying");
        animator.Play("Die"); //away with us
        rb.gravityScale = 1;
        StartCoroutine(Death());
        isStateFinished = true;
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(1);
        var alphaBlendTime = 100f;
        for (var i = 0; i < alphaBlendTime; i++)
        {
            var alpha = 1.0f - i / alphaBlendTime;
            var spriteRender = GetComponent<SpriteRenderer>();
            spriteRender.material.color = new Color(spriteRender.material.color.r, spriteRender.material.color.g,
                spriteRender.material.color.b, alpha);
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }

    protected bool PlayerInSight()
    {
        var hit = Physics2D.CircleCast(enemyCollider.bounds.center, detectionRange, Vector2.right, 0,
            playerLayer); //constant vigilance
        return hit.collider != null;
    }

    protected bool
        PlayerInSightChasing() //double the sight range so enemies will chase a little further than they can spot you
    {
        var hit = Physics2D.CircleCast(enemyCollider.bounds.center, detectionRange * 2, Vector2.right, 0,
            playerLayer); //constant vigilance
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
        if (armoredMeleeAttacks == true && currentState == Behavior.attack)
        {
            enemyCurrentHealth -= damage;
        }
        else if (!inKnockback)
        {
            inKnockback = true; //make sure we're not dealing damage multiple times with one swing
            enemyCurrentHealth -= damage; //deal the damage
            nextState = Behavior.knockback; //change the state
        }
    }

    protected void Flip() //snip
    {
        //Debug.Log("flip");
        movingRight = !movingRight;
        var localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    protected void HitboxActive()
    {
        Debug.Log("hitbox active");
        hitboxActive = true;
        attackLanded = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HitboxInactive()
    {
        Debug.Log("hitbox deactivated");
        hitboxActive = false;
        attackLanded = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    /*
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitboxActive && !attackLanded)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            attackLanded = true;
            hitboxActive = false;
        }
    }
    */
}