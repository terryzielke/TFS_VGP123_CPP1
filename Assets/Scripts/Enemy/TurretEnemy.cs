using System;
using UnityEngine;

public class TurretEnemy : BaseEnemy
{
    [SerializeField] private float fireRate = 1f;
    private float timeSinceLastShot = 0f;

    Shoot shoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        shoot = GetComponent<Shoot>();

        if (fireRate <= 0f)
        {
            fireRate = 1f;
            Debug.LogWarning("TurretEnemy: Fire rate was set to zero or negative. Defaulting to 1 second.");
        }
        shoot.onShotFired += () => timeSinceLastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName("Idle"))
        {
            if(Time.time >= timeSinceLastShot + fireRate)
            {
                anim.SetTrigger("Fire");
                timeSinceLastShot = Time.time;
            }
        }
    }
}
