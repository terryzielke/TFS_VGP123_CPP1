using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

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

    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float jumpForce = 10f;
    [Range(0f, 1f)]
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float fallGravityMultiplier = 2f;   // gravity while falling
    [SerializeField] private float lowJumpGravityMultiplier = 2f; // gravity while rising but NOT holding Jump
    #endregion
    /*
    private float currentPowerupDuration = 0f;
    private float initalJumpForce;
    private Coroutine jumpForceCoroutine;
    */
    #region Component References
    // private and public - public variables are visible in the inspector, private variables are not. Variables are private by default unless otherwise specified.
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    private GroundCheck check;
    private Shoot shoot;
    #endregion

    private int jumpCount = 0;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shoot = GetComponent<Shoot>();

        // Initialize the GroundCheck instance with the required parameters
        check = new GroundCheck(col, rb, groundLayer, groundCheckRadius);

        rb.linearVelocity = Vector2.zero;

        shoot.onShotFired += (velocity) => PlaybackRequest.Instance.RequestOneShotSound(PlaybackRequest.Instance.fireSound, gameObject, PlaybackRequest.Instance.sfxMixerGroup);

    }

    // Update is called once per frame
    void Update()
    {
        // Pause check
        if (GameManager.Instance != null && GameManager.Instance.isPaused) return;

        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        bool isGroundedThisFrame = check.CheckGround();

        float horizontalInput = Input.GetAxis("Horizontal");
        bool fireInput = Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightShift);

        // movement along x axis
        float moveX = horizontalInput * speed;
        rb.linearVelocityX = moveX;

        // Jump pressed this frame
        if (Input.GetButtonDown("Jump") )
        {
            if(jumpCount < maxJumps)
            {
                jumpCount++;
                rb.linearVelocityY = 0f;
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.Log($"Jumped! Jump count: {jumpCount}");
                PlaybackRequest.Instance.RequestOneShotSound(PlaybackRequest.Instance.jumpSound, gameObject, PlaybackRequest.Instance.sfxMixerGroup);
            }
        }
        // Jump released this frame
        if (Input.GetButtonUp("Jump") && rb.linearVelocityY > 0f)
        {
            rb.linearVelocityY *= jumpCutMultiplier;
        }

        if (rb.linearVelocityY < 0f)
        {
            // Falling: pile on extra gravity so the descent isn't floaty.
            // Time.deltaTime keeps it frame-rate independent (bigger step -> bigger nudge).
            rb.linearVelocityY += Physics2D.gravity.y * (fallGravityMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0f && !Input.GetButton("Jump"))
        {
            // Rising but the player let go: fall off faster than a held jump.
            // This overlaps with the jump-cut above; pick one or tune them together.
            rb.linearVelocityY += Physics2D.gravity.y * (lowJumpGravityMultiplier - 1f) * Time.deltaTime;
        }

        // Reset jump count when grounded and falling or stationary
        if (isGroundedThisFrame && rb.linearVelocity.y <= 0)
        {
            jumpCount = 0;
        }

        SpriteFlip(horizontalInput);

        // Update animator parameters
        anim.SetBool("isGrounded", isGroundedThisFrame);
        anim.SetBool("isFalling", rb.linearVelocityY < 0f);
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

    /*
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
    */

    // Player damages enemy when colliding with the "Squish" collider and the player is falling
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

    // Player takes damage when colliding with an enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyProjectile"))
        {
            GameManager.Instance.Lives--;
        }
    }
}
