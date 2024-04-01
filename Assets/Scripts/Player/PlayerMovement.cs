using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public PlayerCombat playerCombat;
    #region transformationVariables
    bool VTransformed;
    bool WTransformed;
    bool HTransformed;


    [SerializeField]
    public float transformCooldown;
    #endregion 

    #region MovmentVariables
    private float horizontal;
    private float vertical;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float jumpingPower = 4f;
    [SerializeField]
    private float dodgeSpeed = 7f;
    private bool dodgeAvailable = true;

    private bool isFacingRight = true;
    [SerializeField]
    private bool isDodging, isJumping;
    private bool canGroundCheck = true;
    public bool canMove = true;
   

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        isJumping = false;
        playerCombat = GetComponent<PlayerCombat>();
        HTransformed = true;

    }

    // Update is called once per frame
    void Update()
    {
        #region Transform Cooldown and DeTransformation
        //transform cooldown increment
        if (transformCooldown>0)
        {
            transformCooldown = transformCooldown - Time.deltaTime;
        }

       //checks your form and the cooldown and automatically detransition you
        if (transformCooldown <= 0 && VTransformed == true)
        {
            VTransformed = false;

            animator.Play("VDetrans");
            //Base form movment values
            dodgeSpeed = 2.5f;
            jumpingPower = 3.4f;
            speed = 1f;
            HTransformed = true;
            playerCombat.attackRange = 0.2f;
        }
        if (transformCooldown <= 0 && WTransformed == true)
        {
            WTransformed = false;

            animator.Play("WDetrans");
            //Base form movment values

            dodgeSpeed = 2.5f;
            jumpingPower = 3.4f;
            speed = 1f;
            HTransformed = true;

        }
        #endregion



        #region animations


        //Animations
        if (isJumping && playerCombat.isAttacking && HTransformed)
        {
            animator.Play("AirAttack");

        }
        if (playerCombat.isAttacking && HTransformed)
        {
            animator.Play("Attack");

        }
        else if (isJumping && HTransformed)
        {
            animator.Play("Jump");

        }
        if (horizontal > 0.1 && isJumping == false && playerCombat.isAttacking && HTransformed || horizontal < -0.1 && isJumping == false && playerCombat.isAttacking && HTransformed)
        {
            animator.Play("Attack");
        }
        else if (horizontal > 0.1 && isJumping == false && !isDodging && HTransformed || horizontal < -0.1 && isJumping == false && !isDodging  && HTransformed)
        {
            animator.Play("Run");
        }
        if (isDodging && HTransformed)
        {
            animator.Play("Dash");
        }
        ///
        if (isJumping && playerCombat.isAttacking && VTransformed)
        {
            animator.Play("VAirAttack");

        }
        if (playerCombat.isAttacking && VTransformed)
        {
            animator.Play("VAttack");

        }
        else if (isJumping && VTransformed)
        {
            animator.Play("VJump");

        }
        if (horizontal > 0.1 && isJumping == false && playerCombat.isAttacking && VTransformed || horizontal < -0.1 && isJumping == false && playerCombat.isAttacking && VTransformed)
        {
            animator.Play("VAttack");
        }
        else if (horizontal > 0.1 && isJumping == false && !isDodging && VTransformed || horizontal < -0.1 && isJumping == false && !isDodging && VTransformed)
        {   
            animator.Play("VWalk");
        }
        if (isDodging && VTransformed)
        {
            animator.Play("VDash");
        }
        ///
        if (isJumping && playerCombat.isAttacking && WTransformed)
        {
            animator.Play("WAirAttack");

        }
        if (playerCombat.isAttacking && WTransformed)
        {
            animator.Play("WAttack");

        }
        else if (isJumping && WTransformed)
        {
            animator.Play("WJump");

        }
        if (horizontal > 0.1 && isJumping == false && playerCombat.isAttacking && WTransformed || horizontal < -0.1 && isJumping == false && playerCombat.isAttacking && WTransformed)
        {
            animator.Play("WAttack");//needs to be change to Walking attack anim
        }
        else if (horizontal > 0.1 && isJumping == false && !isDodging && WTransformed || horizontal < -0.1 && isJumping == false && !isDodging && WTransformed)
        {
            animator.Play("WWalk");
        }
        if (isDodging && WTransformed)
        {
            animator.Play("WDash");
        }


        #endregion


        if (canMove)
        {
            #region Flip Code
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
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
                if (VTransformed)
                {
                    animator.Play("VStand");
                }
                if (WTransformed)
                {
                    animator.Play("WStand");
                }
                
                Debug.Log("Ground");
                isJumping = false;
            }
        }
        #endregion // problem currently where it sets grounded right after jumping
        
    }
    private void FixedUpdate()
    {
        

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
        if(dodgeAvailable)
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
       
        
        
    public void VTransform()
    {
        if (transformCooldown <= 0)
        {
            VTransformed = true;
            HTransformed = false;
            Debug.Log("Vtransform");
            animator.Play("Transform");
            transformCooldown = 10;
            //vampire movment values
            dodgeSpeed = 5;
            jumpingPower = 4.4f;
            speed = 1.5f;
            playerCombat.attackRange = 4f;
            
            
        }
    }



    public void WTransform()
    {
        if (transformCooldown <= 0)
        {
            WTransformed = true;
            HTransformed = false;
            Debug.Log("Wtransform");
            animator.Play("Transform 0");
            transformCooldown = 10;
            //wolf movement values
            dodgeSpeed = 2;
            jumpingPower = 3.4f;
            speed = 0.7f;
            
        }
    }

}
