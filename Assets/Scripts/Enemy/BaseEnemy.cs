using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]

public abstract class BaseEnemy : MonoBehaviour
{

    // Private: variables that are only accessible within this class
    // Public: variables that are accessible from other classes
    // Protected: variables that are accessible within this class and derived classes

    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;

    [SerializeField] protected int maxHealth = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0)
        {
            maxHealth = 1;
            Debug.LogWarning("BaseEnemy: Max health was set to zero or negative. Defaulting to 1.");
        }

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage, DamageType damageType = DamageType.Default)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            // Destroying the game object after a delay to allow death animation to play

            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject, 0.5f);
            }
        }
    }
}

public enum DamageType
{
    Default,
    JumpOn,
    Fire
}