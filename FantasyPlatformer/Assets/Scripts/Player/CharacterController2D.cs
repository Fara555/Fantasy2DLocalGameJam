using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f; // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck; // A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingCheck; // A position marking where to check for ceilings
	[SerializeField] private Collider2D crouchDisableCollider;
	[SerializeField] private PlayerHealth health;
	[SerializeField] private LayerMask whatIsOverrideWhenDoding;

	[HideInInspector] public Animator animator;

	const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool isGrounded;            // Whether or not the player is grounded.
	const float ceilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rb;
	private Transform m_transform;
	[HideInInspector] public bool facingRight = true;  // For determining which way the player is currently facing.

	[Header("Events"), Space(10f)]
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
	public BoolEvent OnCrouchEvent;
	private bool wasCrouching = false;

	[SerializeField] private float jumpForce = 400f;     
	[SerializeField] private float acceleration = 7f; 
	[SerializeField] private float decceleration = 7f;
	[SerializeField] private float velPower = 0.9f;
	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private float frictionAmount;
	[SerializeField] private float jumpCoyoteTime;
	[SerializeField] private float jumpCutMultiplier;
	[SerializeField] private float jumpBufferTime;
	[SerializeField] private float fallGravityMultiplier;
	[SerializeField] private float dodgeForce;
	[SerializeField] private float dodgeTime;
	[SerializeField] private float dodgeCooldown = 1f;
	private float gravityScale = 5f;
	private float lastGroundedTime;
    private float lastJumpTime;

	[HideInInspector] public bool dodgeOnCooldown;
	

    private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		m_transform = GetComponent<Transform>();
		animator = GetComponent<Animator>();	

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		gravityScale = rb.gravityScale;

		dodgeOnCooldown = false;
	}


    private void FixedUpdate()
	{
		bool wasGrounded = isGrounded;
		isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				if (wasGrounded)
					OnLandEvent.Invoke();
            }
		}
	}

	public void Move(float move, bool crouch, bool jump, bool cutJump, bool dodge)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
			{
				crouch = true;
			}
		}

		if (dodge && !crouch && !dodgeOnCooldown)
		{
			StartCoroutine(ExecuteDodge());
		}

		//only control the player if grounded or airControl is turned on
		if (isGrounded || airControl)
		{

			// If crouching
			if (crouch)
			{
				if (!wasCrouching)
				{
					wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= crouchSpeed;

				// Disable one of the colliders when crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = true;

				if (wasCrouching)
				{
					wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

            #region Movement

            //calculate the direction we want to move in and our disered velocity
            float targetSpeed = move * moveSpeed;
			//calculate difference between current and disired velocity 
			float speedDif = targetSpeed - rb.velocity.x;
			//change acceleration rate depending on situation
			float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
			//applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
			//finally multiplies by sign to reapply direction
			float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

			//apllies force to rigidbody, multyplying by Vector2.right so that it only affects X axis
			rb.AddForce(movement * Vector2.right);

            #endregion

            #region Fricition

            if (lastGroundedTime > 0f && Mathf.Abs(move) < 0.01f) 
            {
				//the
				float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
				//sets to movement direction
				amount *= Mathf.Sign(rb.velocity.x);
				//applies force against movement direction
				rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }

            #endregion

            #region Flip Player Sprite

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
			{
				// ... flip the player.
				Flip(m_transform);
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight)
			{
				// ... flip the player.
				Flip(m_transform);
			}

            #endregion

            #region Jump

            if (jump)
            {
				lastJumpTime = jumpBufferTime;
            }
            else
            {
				lastJumpTime -= Time.deltaTime;
            }

			if (isGrounded)
			{
				lastGroundedTime = jumpCoyoteTime;
			}
			else
			{
				lastGroundedTime -= Time.deltaTime;
			}

			if (lastGroundedTime > 0f && lastJumpTime > 0f && isGrounded)
            {
				Jump();
            }
            if (cutJump && rb.velocity.y > 0f)
            {
                if (!isGrounded)
                {
					CutJump();
                }
            }


			#endregion

			#region Jump Gravity

			if (rb.velocity.y < 0)
			{
				rb.gravityScale = gravityScale * fallGravityMultiplier;
			}
			else
			{
				rb.gravityScale = gravityScale;
			}

			#endregion
		} 
    }

	private void Flip(Transform transform)
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void Jump()
	{
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //Add force to the player when he jumps 
		lastGroundedTime = 0f;
		lastJumpTime = 0f;
		isGrounded = false;
	}

	private void CutJump()
	{
		rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);//Add force to the player including CutMultiplayer when he cut jumps
		lastGroundedTime = 0f;
		lastJumpTime = 0f;
		isGrounded = false;
	}

	private IEnumerator ExecuteDodge()
	{
		Dodge();
		yield return new WaitForSeconds(dodgeTime);
		StartCoroutine(SetDodgeOnCooldown());
    }

	private void Dodge()
	{
        animator.SetBool("Dodge", true);

        rb.excludeLayers = whatIsOverrideWhenDoding; // Make player phase thru specific layer
        dodgeOnCooldown = true;
        health.invincible = true;

        if (facingRight) rb.AddForce(Vector3.right * dodgeForce, ForceMode2D.Impulse);
        else rb.AddForce(Vector3.left * dodgeForce, ForceMode2D.Impulse);
    }

    private IEnumerator SetDodgeOnCooldown()
	{
        animator.SetBool("Dodge", false);
        health.invincible = false;
		rb.excludeLayers = 0; // Return to nothing
        yield return new WaitForSeconds(dodgeCooldown);
		dodgeOnCooldown = false;
	}
}