using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetVolocity(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }

    // collision detection for the projectile
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Projectile: Collided with {collision.gameObject.name} tagged as {collision.gameObject.tag}");
        
        if(collision.gameObject.name == "Edge" || collision.gameObject.name == "Box" || collision.gameObject.name == "Floor")
        {
            //Debug.Log("Projectile hit the ground.");
            Destroy(gameObject); // Destroy the projectile after hitting the ground
        }

        if(collision.gameObject.CompareTag("Enemy") && transform.gameObject.CompareTag("PlayerProjectile"))
        {
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
            if(enemy != null) {
                enemy.TakeDamage(damage);
                Destroy(gameObject); // Destroy the projectile after hitting an enemy
            }
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
