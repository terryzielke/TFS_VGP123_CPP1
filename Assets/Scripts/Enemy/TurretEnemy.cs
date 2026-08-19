using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Shoot), typeof(SpriteRenderer))]
public class TurretEnemy : BaseEnemy
{
    [SerializeField] private float fireRate = 1f;
    private float timeSinceLastShot = 0f;

    private float fireDistance = 10f; // The distance within which the turret will fire at the player

    Shoot shoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        shoot = GetComponent<Shoot>();

        // Ensure fireRate is not zero or negative to avoid division by zero or unexpected behavior
        if (fireRate <= 0f)
        {
            fireRate = 1f;
            Debug.LogWarning("TurretEnemy: Fire rate was set to zero or negative. Defaulting to 1 second.");
        }
        shoot.onShotFired += (velocity) => timeSinceLastShot = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName("Idle"))
        {
            timeSinceLastShot += Time.deltaTime;
            if(timeSinceLastShot >= fireRate)
            {
                if (distanceToPlayer < fireDistance)
                {
                    anim.SetTrigger("Fire");
                }
            }
        }

        // If player X position is greater than turret X position, flip the sprite to face right, else face left
        if (player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            if (direction.x > 0)
            {
                sr.flipX = false;
            }
            else if (direction.x < 0)
            {
                sr.flipX = true;
            }
        }
    }
}
