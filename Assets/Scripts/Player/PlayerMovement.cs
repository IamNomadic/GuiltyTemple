using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Public Variables
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    public PlayerInputs playerInputs;
    #endregion
    #region Private Variables
    private float horizontal;
    private float vertical;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float jumpingPower = 4f;
    private bool isFacingRight = true;
    [SerializeField]
    private float dodgeSpeed = 7f;
    private bool dodgeAvailable = true;
    private bool isDodging;

    private InputAction move;
    private InputAction jump;
    #endregion

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }
    private void OnEnable()
    {
        move = playerInputs.Player.Move;
        move.Enable();
        jump = playerInputs.Player.Jump;
        jump.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    void FixedUpdate()
    {
	    animator.SetFloat("Speed", Mathf.Abs(horizontal));
	    animator.SetFloat("SpeedY", Mathf.Abs(rb.velocity.y));

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

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if(isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        if (jump.triggered && IsGrounded())
            Jump();
	    ///// Temporary input code
	    if (Input.GetKeyDown(KeyCode.Q))
	    {
	        animator.SetBool("WTransforming", true);
	    }
	    if (Input.GetKeyDown(KeyCode.E))
	    {
	        animator.SetBool("VTransforming", true);
	    }
	    if (Input.GetKeyDown(KeyCode.F))
	    {
	        animator.SetBool("VTransforming", false);
	        animator.SetBool("WTransforming", false);
	    }   
	    ////////

	    if (IsGrounded())
	    {
            

        }

    }
    
    public void Jump()
    {
	    animator.SetBool("IsJumping", true);
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        IEnumerator JumpWaitTime()
        {
            yield return new WaitForSeconds(0.1f);
            animator.SetBool("IsJumping", false);
        }
        StartCoroutine(JumpWaitTime());
    }

    private bool IsGrounded()
    {
	 
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    

    public void Dodge (InputAction.CallbackContext context)
    {
        if(context.performed && dodgeAvailable)
        {
            isDodging = true;
            dodgeAvailable = false;
	        animator.SetBool("IsDashing", true);
        }
        IEnumerator DodgeTimerCoroutine()
        {
            yield return new WaitForSeconds(.2f);// Wait a sec
            isDodging = false;
            dodgeAvailable = true;
	        animator.SetBool("IsDashing", false);
        }
        StartCoroutine(DodgeTimerCoroutine());
    }
    public void Move (InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        
    }

}
