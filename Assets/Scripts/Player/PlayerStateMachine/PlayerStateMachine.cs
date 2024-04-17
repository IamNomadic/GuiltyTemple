using System.Collections;
using UnityEngine;

//this is the context file for all the states !! IMPORTANT FILE !!
public class PlayerStateMachine : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public PlayerInputs playerInputs;
    private PlayerStateFactory states;

    private void Awake()
    {
        //get all components here instead of using the inspector <3
        //also get all the other stuff to make the code work i dunno what u guys did for the movement and stuff

        //setup state
        states = new PlayerStateFactory(this);
        CurrentState = states.HumanS();
        CurrentState.EnterState();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Move(Vector2 context)
    {
        horizontal = context.x;
        vertical = context.y;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            isJumping = true;
            canGroundCheck = false;

            Invoke("EnableGroundCheck", .1f);
        }
    }

    private void EnableGroundCheck()
    {
        canGroundCheck = true;
    }

    public void JumpCanceled()
    {
        Debug.Log("canceled");

        if (rb.velocity.y > 0f) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer) && canGroundCheck;
    }

    private IEnumerator DodgeTimerCoroutine()
    {
        yield return new WaitForSeconds(.2f); // Wait a sec
        isDodging = false;
        dodgeAvailable = true;
    }

    public void Dodge()
    {
        Debug.Log("Dodge!");
        if (dodgeAvailable)
        {
            isDodging = true;
            dodgeAvailable = false;
            animator.Play("Dash");
        }

        StartCoroutine(DodgeTimerCoroutine());
    }

    private IEnumerator AttackWaitTime()
    {
        yield return new WaitForSeconds(0.55f);
        isAttacking = false;
    }

    public void Attack()
    {
        if (!ph.dead) isAttacking = true;
        animator.Play("Attack");
        var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " was hit.");

            enemy.GetComponent<Enemy>().TakeDamage(playerATKDamage);
        }


        StartCoroutine(AttackWaitTime());
    }

    #region MovmentVariables

    private float horizontal;
    private float vertical;

    [SerializeField] private float speed = 2f;

    [SerializeField] private float jumpingPower = 4f;

    [SerializeField] private float dodgeSpeed = 7f;

    private bool dodgeAvailable = true;

    private bool isFacingRight = true;

    [SerializeField] private bool isDodging, isJumping;

    private bool canGroundCheck = true;
    public bool canMove = true;

    #endregion

    #region CombatVariables

    public Transform attackPoint;
    public PlayerHealth ph;

    [SerializeField] public float attackRange = 0.2f;

    public LayerMask enemyLayers;
    public bool isAttacking;
    public int playerATKDamage = 1;

    #endregion

    #region Getters and Setters

    public PlayerBaseState CurrentState { get; set; }

    public Transform AttackPoint => attackPoint;
    public PlayerHealth PlayerHealth => ph;
    public float AttackRange => attackRange;
    public LayerMask EnemyLayers => enemyLayers;

    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }

    public int PlayerATKDamage => playerATKDamage;

    #endregion
}