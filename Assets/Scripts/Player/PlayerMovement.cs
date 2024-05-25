using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private CharacterController2D controller;

    [Header("Other")]
    [SerializeField] private float runSpeed = 40f;
    [HideInInspector] public Animator animator;

    [HideInInspector] public float horizontalMove = 0f;
    private Rigidbody2D rb;

    //Movement
    private bool jump = false;
    private bool cutJump = false;
    private bool crouch = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //Input from player to run
        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))  //Inputs from player to jump
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) //Input from player to cut jump
        {
            cutJump = true;
        }

        if (Input.GetButtonDown("Crouch")) //Input from player for crouch
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, cutJump); //Move method
        jump = false;
        cutJump = false;
    }

    public void OnLanding()//Method for landing event animation
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)//Method for crouching event animation
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
}