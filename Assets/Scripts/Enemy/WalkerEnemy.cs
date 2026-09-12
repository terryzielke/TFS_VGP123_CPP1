using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class WalkerEnemy : BaseEnemy
{
    [SerializeField] private float xVal = 2f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

    }

    public override void TakeDamage(int damage, DamageType damageType = DamageType.Default)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Death") || stateInfo.IsName("Squish")) return;

        if(damageType == DamageType.JumpOn)
        {
            //anim.SetTrigger("Squish");
            //Destroy(transform.parent.gameObject, 0.5f);
        }

        base.TakeDamage(1, damageType);

        PlaybackRequest.Instance.RequestOneShotSound(PlaybackRequest.Instance.popSound, gameObject, PlaybackRequest.Instance.sfxMixerGroup);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Walk"))
        {
            //if(!sr.flipX) rb.linearVelocityX = xVal;
            //else rb.linearVelocityY = xVal;

            // Ternary operator to set the velocity based on the sprite's flip state

            rb.linearVelocityX = sr.flipX ? -xVal : xVal;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            //anim.SetTrigger("Turn");
            sr.flipX = !sr.flipX;
        }
    }
}
