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
    private int maxJumps = 2;
    #endregion

    #region Components
    // private and public - public variables are visible in the inspector, private variables are not. Variables are private by default unless otherwise specified.
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    #endregion

    #region Ground Check Stuff
    // Ground check variables
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckRadius = 0.2f;

    // Ground check position is calculated based on the collider's bounds
    private Vector2 groundCheckPos => CalculateGroundCheckPos();
    private bool isGrounded;
    private bool isCrouching = false;
    private int jumpCount = 0;

    // Foot position helper to calculate the ground check position based on the collider's bounds
    private Vector2 CalculateGroundCheckPos()
    {
        Bounds bounds = col.bounds;
        return new Vector2(bounds.center.x, bounds.min.y);
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.linearVelocity = Vector2.zero;

        /*
         * Stale code for creating a ground check transform, but it is not needed since we are calculating the ground check position based on the collider's bounds
         * 
        if(groundCheckTransform == null)
        {
            groundCheckTransform = new GameObject("GroundCheck").transform;
            groundCheckTransform.SetParent(transform);
            groundCheckTransform.localPosition()
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

        // Crouch input
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }


        if (rb.linearVelocity.y < 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
        }

        float horizontalInput = Input.GetAxis("Horizontal");

        float moveX = horizontalInput * speed;

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
                isGrounded = false;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log($"Jumped! Jump count: {jumpCount}");
            }
        }

        if (isGrounded)
        {
            jumpCount = 0;
        }

        SpriteFlip(horizontalInput);

        // Update animator parameters
        anim.SetBool("isGrounded", isGrounded);
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

    // single line flipper
    private void SpriteFlip(float horizontalInput) => sr.flipX = (horizontalInput < 0);
    /*
    if (sr.flipX && horizontalInput > 0)
    {
        sr.flipX = false;
    }
    else if (!sr.flipX && horizontalInput < 0 || sr.flipX && horizontalInput > 0)
    {
        sr.flipX = true;
    }
    */
}
