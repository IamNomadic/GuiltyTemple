using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public static event Action OnPlayerDamaged;

    [HideInInspector] public Transform groundCheck;
    [HideInInspector] public LayerMask groundLayer;
    Animator animator;
    PlayerCombat playerCombat;
    PlayerHealth playerHealth;
    
    #region transformationVariables
     public bool VTransformed;
     public bool WTransformed;
    [HideInInspector] public bool HTransformed;
    [SerializeField] float cooldown;
    [Header("Vampire")]
    //Vampire Character values
    [SerializeField] int vHP;
    [SerializeField] float vWalkSpeed;
    [SerializeField] float vJumpPower;
    [SerializeField] float vDodgeSpeed;
    [SerializeField] float vAttackRange;
    [SerializeField] float vCooldown;
    [Header("Wolf")]
    //Wolf Character values
    [SerializeField] int wHP;
    [SerializeField] float wWalkSpeed;
    [SerializeField] float wJumpPower;
    [SerializeField] float wDodgeSpeed;
    [SerializeField] float wAttackRange;
    [SerializeField] float wCooldown;
    [Header("Human")]
    //Base Characer values
    [SerializeField] int hHP;
    [SerializeField] public float hWalkSpeed;
    [SerializeField] float hJumpPower;
    [SerializeField] float hDodgeSpeed;
    [SerializeField] float hAttackRange;
    
    #endregion 

    #region MovmentVariables
    private float horizontal;
    private float vertical;
    private float walkSpeed = 2f;
    private float jumpPower = 4f;
    private float dodgeSpeed = 7f;
    private bool dodgeAvailable = true;
    private bool isFacingRight = true;
    [SerializeField]
    private bool isDodging, isJumping;
    private bool canGroundCheck = true;
    public bool canMove = true;
    bool isTransforming;
   

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        //getting our components

        playerCombat = GetComponent<PlayerCombat>();
        playerHealth = GetComponent<PlayerHealth>();
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // setting character values to default 
        HTransformed = true;
        walkSpeed = hWalkSpeed;
        jumpPower = hJumpPower;
        dodgeSpeed = hDodgeSpeed;
        playerCombat.attackRange = hAttackRange;
        playerHealth.maxHealth = hHP;
        cooldown = 0;
        isJumping = false;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        #region Transform Cooldown and DeTransformation
        //transform cooldown increment
        if (cooldown>0)
        {
            cooldown = cooldown - Time.deltaTime;
        }

       //checks your form and the cooldown and automatically detransition you
        if (cooldown <= 0 && VTransformed == true)
        {
            VDetrans(); 
           

        }
        if (cooldown <= 0 && WTransformed == true)
        {
            WDetrans(); 

        }
        #endregion



        #region animations


        //Animations
        if (canMove && isJumping && playerCombat.isAttacking && HTransformed)
        {
            animator.Play("AirAttack");
        }
        else if (canMove && horizontal > 0.1 && isJumping == false && playerCombat.isAttacking && HTransformed || canMove && horizontal < -0.1 && isJumping == false && playerCombat.isAttacking && HTransformed)
        {
            animator.Play("WalkingAttack");
        }
        else if (canMove && horizontal < 0.1 && playerCombat.isAttacking && HTransformed || canMove && horizontal > -0.1 && playerCombat.isAttacking && HTransformed)
        {
            animator.Play("Attack");
        }
        else if (canMove && isJumping && HTransformed && !isDodging)
        {
            animator.Play("Jump");
        }
        else if (canMove && horizontal > 0.1 && isJumping == false && !isDodging && HTransformed || canMove && horizontal < -0.1 && isJumping == false && !isDodging  && HTransformed)
        {
            animator.Play("Run");
        }
        else if (canMove && horizontal < 0.1 && isJumping == false && !isDodging && !playerCombat.isAttacking && HTransformed || canMove && horizontal > -0.1 && isJumping == false && !isDodging && !playerCombat.isAttacking && HTransformed)
        {
            animator.Play("Stand");
        }
        if (canMove && isDodging && HTransformed)
        {
            animator.Play("Dash");
        }
        ///vampire anims
        if (canMove && isJumping && playerCombat.isAttacking && VTransformed)
        {
            animator.Play("VAirAttack");
        }
        else if (canMove && horizontal > 0.1 && isJumping == false && playerCombat.isAttacking && VTransformed || canMove && horizontal < -0.1 && isJumping == false && playerCombat.isAttacking && VTransformed)
        {
            animator.Play("VWalkingAttack");
        }
        else if (playerCombat.isAttacking && VTransformed)
        {
            animator.Play("VAttack");
        }
        else if (canMove && isJumping && VTransformed && !isDodging)
        {
            animator.Play("VJump");
        }
        else if (canMove && horizontal > 0.1 && isJumping == false && !isDodging && VTransformed || canMove && horizontal < -0.1 && isJumping == false && !isDodging && VTransformed)
        {   
            animator.Play("VWalk");
        }
        else if (canMove && horizontal < 0.1 && isJumping == false && !isDodging && !playerCombat.isAttacking && VTransformed || canMove && horizontal > -0.1 && isJumping == false && !isDodging&& !playerCombat.isAttacking && VTransformed)
        {
            animator.Play("VStand");
        }
        if (canMove && isDodging && VTransformed)
        {
            animator.Play("VDash");
        }
        /// wolf anims
        if (canMove && isJumping && playerCombat.isAttacking && WTransformed)
        {
            animator.Play("WAirAttack");
        }
        else if (canMove && horizontal > 0.1 && isJumping == false && playerCombat.isAttacking && WTransformed || canMove && horizontal < -0.1 && isJumping == false && playerCombat.isAttacking && WTransformed)
        {
            animator.Play("WWalkingAttack");
        }
        else if (canMove && playerCombat.isAttacking && WTransformed)
        {
            animator.Play("WAttack");
        }
        else if (canMove && isJumping && WTransformed && !isDodging)
        {
            animator.Play("WJump");
        }
        else if (canMove && horizontal > 0.1 && isJumping == false && !isDodging && WTransformed || canMove && horizontal < -0.1 && isJumping == false && !isDodging && WTransformed)
        {
            animator.Play("WWalk");
        }
        else if (canMove && horizontal < 0.1 && isJumping == false && !isDodging && !playerCombat.isAttacking && WTransformed || canMove && horizontal > -0.1 && isJumping == false && !isDodging && !playerCombat.isAttacking && WTransformed)
        {
            animator.Play("WStand");
        }
        if (canMove && isDodging && WTransformed)
        {
            animator.Play("WDash");
        }



        #endregion



        #region Flip Code
        if (canMove)
        {
            
            if (!isFacingRight && horizontal > 0f)
            {
                Flip();
            }
            else if (isFacingRight && horizontal < 0f)
            {
                Flip();
            }
        }
        #endregion



        #region Dodge Code
        if (canMove)
        {
            if (!isDodging)
            {
                rb.velocity = new Vector2(horizontal * walkSpeed, rb.velocity.y);
            }

            if (isDodging)
            {
                if (!isFacingRight)
                {
                    rb.velocity = new Vector2(-dodgeSpeed, rb.velocity.y);
                }
                else if (isFacingRight)
                {
                    rb.velocity = new Vector2(dodgeSpeed, rb.velocity.y);
                }
            }
        }
        #endregion



        #region Grounded Code
        if (canMove)
        {
            if (IsGrounded() && isJumping == true)
            {
                if (HTransformed)
                {
                    animator.Play("Stand");
                }
                else if (VTransformed)
                {
                    animator.Play("VStand");
                }
                else if (WTransformed)
                {
                    animator.Play("WStand");
                }
                
                Debug.Log("Ground");
                isJumping = false;
            }
        }
        #endregion // problem currently where it sets grounded right after jumping
        
    }
    public void Move(Vector2 context)
    {
        horizontal = context.x;
        vertical = context.y;
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    public void Jump()
    {
        if (IsGrounded() && canMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
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

        if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }      
    }
    private bool IsGrounded()
    {

        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer) && canGroundCheck;
    }
    public void Dodge ()
    {
        Debug.Log("Dodge!");
        if(dodgeAvailable && canMove)
        {
            isDodging = true;
            dodgeAvailable = false;
            animator.Play("Dash");
        }
        IEnumerator DodgeTimerCoroutine()
        {
            yield return new WaitForSeconds(.3f);// Wait a sec
            isDodging = false;
            dodgeAvailable = true;
	   
        }
        StartCoroutine(DodgeTimerCoroutine());
    }      
    public void VDetrans()
    {
        canMove = false;
        VTransformed = false;

        animator.Play("VDetrans");
        //Base form values
        playerHealth.maxHealth = hHP;

        walkSpeed = hWalkSpeed;
        jumpPower = hJumpPower;
        dodgeSpeed = hDodgeSpeed;
        playerCombat.attackRange = hAttackRange;
        HTransformed = true;
        StartCoroutine(VDetrans());
        IEnumerator VDetrans()
        {
            yield return new WaitForSeconds(0.55f);
            canMove = true;
            OnPlayerDamaged?.Invoke();

        }
    }
    public void WDetrans()
    {
        WTransformed = false;
        canMove = false;

        animator.Play("WDetrans");
        //Base form values
        playerHealth.maxHealth = hHP;
        walkSpeed = hWalkSpeed;
        jumpPower = hJumpPower;
        dodgeSpeed = hDodgeSpeed;
        playerCombat.attackRange = hAttackRange;
        HTransformed = true;
        StartCoroutine(WDetrans());
        IEnumerator WDetrans()
        {
            yield return new WaitForSeconds(0.55f);
            canMove = true;
            OnPlayerDamaged?.Invoke();

        }
    }


    public void VTransform()
    {
        if (cooldown <= 0)
        {
            StartCoroutine(VTransforming());
            // transforms correct
            VTransformed = true;
            WTransformed = false;
            HTransformed = false;
            canMove = false;
            Debug.Log("Vtransform");
            animator.Play("Transform");
            cooldown = vCooldown;
            //vampire movment values
            playerHealth.maxHealth = vHP;
            walkSpeed = vWalkSpeed;
            jumpPower = vJumpPower;
            dodgeSpeed = vDodgeSpeed;
            playerCombat.attackRange = vAttackRange;
        }
        IEnumerator VTransforming()
        {
            yield return new WaitForSeconds(0.55f);
            canMove = true;
            OnPlayerDamaged?.Invoke();
        }
    }
    public void WTransform()
    {
        if (cooldown <= 0)
        {
            StartCoroutine(WTransforming());
            // transforms correct
            WTransformed = true;
            HTransformed = false;
            VTransformed = false;
            canMove = false;
            Debug.Log("Wtransform");
            animator.Play("Transform 0");
            cooldown = wCooldown;
            //wolf movement values
            playerHealth.maxHealth = wHP;
            walkSpeed = wWalkSpeed;
            jumpPower = wJumpPower;
            dodgeSpeed = wDodgeSpeed;
            playerCombat.attackRange = wAttackRange;
        }
        IEnumerator WTransforming()
        {
            yield return new WaitForSeconds(0.55f);
            canMove = true;
            OnPlayerDamaged?.Invoke();

        }
        
    }

}
