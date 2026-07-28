using System.Runtime.CompilerServices;
using UnityEngine;

// <summary>
// Responsible for taking the imput and applying it ot the rigidbody component of the player object
// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class TFS_PlayerController : MonoBehaviour
{
    #region Tunalble Variables
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private int maxJumps = 1;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    #endregion

    #region Components
    // private and public - public variables are visible in the inspector, private variables are not. Variables are private by default unless otherwise specified.
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    //private GroundCheck check;
    private GroundCheck1 check;
    #endregion

    private int jumpCount = 0;
    private bool isCrouching = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Initialize the GroundCheck1 instance with the required parameters
        check = new GroundCheck1(col, rb, groundLayer, groundCheckRadius);

        rb.linearVelocity = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {

        bool isGroundedThisFrame = check.CheckGround();

        float horizontalInput = Input.GetAxis("Horizontal");

        float moveX = horizontalInput * speed;

        // Crouch input
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        if(isCrouching)
        {
            rb.linearVelocityX = 0f;
        }
        else
        {
            rb.linearVelocityX = moveX;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(jumpCount < maxJumps)
            {
                jumpCount++;
                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log($"Jumped! Jump count: {jumpCount}");
            }
        }

        if (isGroundedThisFrame && rb.linearVelocity.y <= 0)
        {
            jumpCount = 0;
        }

        SpriteFlip(horizontalInput);

        // Update animator parameters
        anim.SetBool("isGrounded", isGroundedThisFrame);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));

        // Attack input
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightShift))
        {
            // Reset trigger
            anim.ResetTrigger("attackTrigger");
            // Fire the attack trigger
            anim.SetTrigger("attackTrigger");
        }

    }

    // flip the sprite based on the horizontal input
    private void SpriteFlip(float horizontalInput)
    {
        if(sr.flipX && horizontalInput > 0 || !sr.flipX && horizontalInput < 0)
        {
            sr.flipX = !sr.flipX;
        }
    }

}
