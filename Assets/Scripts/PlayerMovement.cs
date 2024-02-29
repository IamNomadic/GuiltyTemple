using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;
    
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    }
    public void jump(InputAction.CallbackContext context)
    {

        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
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
    public void Transform (InputAction.CallbackContext context)
	{
	if(context.performed)
		{
		animator.SetBool("VTransforming", true);
		}
	}
}
