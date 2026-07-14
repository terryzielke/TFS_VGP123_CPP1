using UnityEngine;

// <summary>
// Responsible for taking the imput and applying it ot the rigidbody component of the player object
// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class TFS_PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float jumpForce = 10f;

    // private and public - public variables are visible in the inspector, private variables are not. Variables are private by default unless otherwise specified.
    [SerializeField]
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float moveX = horizontalInput * speed; // 5f is the speed of the player

        rb.linearVelocityX = moveX;

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }
}
