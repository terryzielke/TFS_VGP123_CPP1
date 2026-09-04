using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Destroy(gameObject, lifetime);
    }
    private float age;
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPaused) return;

        // Time.deltaTime is 0 when timeScale is 0, so this also naturally freezes —
        // but the explicit guard above makes the intent obvious to a reader.
        age += Time.deltaTime;
        if (age >= lifetime) Destroy(gameObject);
    }

    public void SetVolocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }

    // collision detection for the projectile
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision.gameObject is the thing we hit. Grab it once for readability.
        GameObject other = collision.gameObject;

        // CompareTag is the idiomatic Unity check: faster than `other.tag == "..."`
        // (no string allocation) and it throws if the tag doesn't exist, which
        // catches typos. Now ANY object tagged "Obstacle" stops the projectile.
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Enemy") && CompareTag("PlayerProjectile"))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Player") && CompareTag("EnemyProjectile"))
        {

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    //collision detection functions for trigger colliders - less restrictions on colliding bodies because these colliders do not block collisions - but they are still useful for things like pickups (hint hint)
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
}
