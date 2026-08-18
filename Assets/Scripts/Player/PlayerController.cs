using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

// <summary>
// Responsible for taking the imput and applying it ot the rigidbody component of the player object
// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Tunalble Variables
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float speed = 5f;

    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpForcePowerup = 20f;
    [SerializeField] private float jumpForcePowerupDuration = 5f; 
    [SerializeField] private int maxJumps = 1;
    #endregion

    private float currentPowerupDuration = 0f;
    private float initalJumpForce;

    private Coroutine jumpForceCoroutine;

    #region Component References
    // private and public - public variables are visible in the inspector, private variables are not. Variables are private by default unless otherwise specified.
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    private GroundCheck1 check;
    #endregion

    private int jumpCount = 0;
    public int maxLives = 3;
    private int currentLives;

    // C++ style getter and setter for lives
    /*
    public void setLives(int value)
    {
        if(value >= maxLives)
        {
            currentLives = maxLives;
        }
        else if(value <= 0)
        {
            currentLives = 0;
            // game over logic here
        }
        else
        {
            currentLives = value;
        }
    }

    // Get lives function
    public int getLives() { return currentLives; }
    */

    // C# style property for lives
    private int _lives = 3;
    public int Lives
    {
        get { return _lives; }
        set
        {
            if (value >= maxLives)
            {
                _lives = maxLives;
            }
            else if (value <= 0)
            {
                _lives = 0;
                Debug.Log("Game Over!");
            }
            else
            {
                _lives = value;
            }

            Debug.Log($"Lives set to: {_lives}");
        }
    }

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
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        bool isGroundedThisFrame = check.CheckGround();

        float horizontalInput = Input.GetAxis("Horizontal");
        bool isCrouching = false;
        bool fireInput = Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightShift);

        // movement along x axis
        float moveX = horizontalInput * speed;
        rb.linearVelocityX = moveX;

        
        // Crouch input
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocityX = 0f;
            isCrouching = true;
        }
        

        // Jump input
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

        /*
        if (clipInfo[0].clip.name == "Attack")
        {
            rb.linearVelocityX = 0;
        }
        */

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
    
    public void StartJumpForceChange()
    {
        if (jumpForceCoroutine != null)
        {
            StopCoroutine(jumpForceCoroutine);
            jumpForceCoroutine = null;
            jumpForce = initalJumpForce;
        }
        jumpForceCoroutine = StartCoroutine(JumpForcePowerupCoroutine());
    }

    IEnumerator JumpForcePowerupCoroutine()
    {
        initalJumpForce = jumpForce;
        jumpForce = jumpForcePowerup;
        currentPowerupDuration = jumpForcePowerupDuration;
        while (currentPowerupDuration > 0)
        {
            currentPowerupDuration -= Time.deltaTime;
            Debug.Log("Jump force power-up active. Time remaining: " + currentPowerupDuration.ToString("F2") + " seconds.");
            yield return null;
        }
        jumpForce = initalJumpForce;
        jumpForceCoroutine = null;
        currentPowerupDuration = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( (collision.CompareTag("Squish") && rb.linearVelocityY < 0))
        {
            BaseEnemy enemy = collision.GetComponentInParent<BaseEnemy>();
            if (enemy != null) {
                enemy.TakeDamage(0, DamageType.JumpOn);
                rb.linearVelocityY = 0;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
