using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerMovement.cs
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    
    private float horizontal;
=======
	
    public Rigidbody2D Rigidbody;
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public Animator Animator;
    
    public PlayerInputs PlayerControls;
    private InputAction move;
    private InputAction jump;
    
    private float horizontal;
    private float vertical;
    Vector2 moveDirection = Vector2.zero;
[SerializeField]
>>>>>>> Stashed changes:Assets/Scripts/PlayerMovement.cs
    private float speed = 2f;
    private float jumpingPower = 4f;
    private bool isFacingRight = true;

    private float dodgeSpeed = 7f;
    private bool dodgeAvailable = true;
    private bool isDodging;

    private void Awake()
    {
        PlayerControls = new PlayerInputs();
    }
    private void OnEnable()
    {
        move = PlayerControls.Player.Move;
        move.Enable();
        jump = PlayerControls.Player.Jump;
        jump.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerMovement.cs
=======
	Animator.SetFloat("Speed", Mathf.Abs(horizontal));
	Animator.SetFloat("SpeedY", Mathf.Abs(Rigidbody.velocity.y));

>>>>>>> Stashed changes:Assets/Scripts/PlayerMovement.cs
        if (!isDodging)
        {
            Rigidbody.velocity = new Vector2(horizontal * speed, Rigidbody.velocity.y);
        }

        if (isDodging)
        {
            if (!isFacingRight)
            {
                Rigidbody.velocity = new Vector2(-dodgeSpeed, Rigidbody.velocity.y);
            }
            else if (isFacingRight)
            {
                Rigidbody.velocity = new Vector2(dodgeSpeed, Rigidbody.velocity.y);
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
    public void Jump()
    {
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerMovement.cs
        if (context.performed && IsGrounded())
=======

        if (jump.triggered && IsGrounded())
>>>>>>> Stashed changes:Assets/Scripts/PlayerMovement.cs
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jumpingPower);
        }
        if (!jump.triggered && Rigidbody.velocity.y > 0f)
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerMovement.cs
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
=======
	 
        return Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
>>>>>>> Stashed changes:Assets/Scripts/PlayerMovement.cs
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
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerMovement.cs
=======
	    Animator.SetBool("IsDashing", true);
>>>>>>> Stashed changes:Assets/Scripts/PlayerMovement.cs
        }
        IEnumerator DodgeTimerCoroutine()
        {
            yield return new WaitForSeconds(.1f);// Wait a sec
            isDodging = false;
            dodgeAvailable = true;
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerMovement.cs
=======
	    Animator.SetBool("IsDashing", false);
>>>>>>> Stashed changes:Assets/Scripts/PlayerMovement.cs
        }
        StartCoroutine(DodgeTimerCoroutine());
    }
    public void Move()
    {
        moveDirection = move.ReadValue<Vector2>();
    }
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerMovement.cs
=======
    public void Transform (InputAction.CallbackContext context)
	{
	if(context.performed)
		{
		Animator.SetBool("VTransforming", true);
		}
	}
>>>>>>> Stashed changes:Assets/Scripts/PlayerMovement.cs
}
