using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private Transform spawnPointBullet;
    [SerializeField] private Projectile projectilePrefab;

    private Vector2 leftShotVelocity;
    public Action<Vector2> onShotFired;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Only the reference checks are still meaningful now.
        if (spawnPointBullet == null || projectilePrefab == null)
        {
            Debug.LogError("Shoot: assign spawnPointBullet and projectilePrefab in the Inspector.");
        }
    }

    public void Fire()
    {
        if (spawnPointBullet == null || projectilePrefab == null)
        {
            Debug.LogError("Shoot: missing required references.");
            return;
        }

        // spawnPointBullet is a child of the Arm, and ArmAim keeps the Arm rotated
        // toward the cursor. Transform.right is an object's LOCAL +X axis expressed
        // in world space - so this vector already points at the cursor, for free.
        // It's a Vector3; assigning to Vector2 implicitly drops the z component.
        Vector2 dir = spawnPointBullet.right;

        // Direction is a unit vector, so multiplying by the scalar speed gives the
        // full velocity. Direction and speed are now cleanly separated.
        Vector2 velocity = dir * projectileSpeed;

        // Point the projectile along its travel direction. Atan2(y, x) gives the
        // angle of the vector in radians from +X; Rad2Deg converts it because
        // Quaternion.Euler wants degrees. 2D rotation is always around Z.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        Projectile curProjectile = Instantiate(projectilePrefab, spawnPointBullet.position, rot);
        curProjectile.SetVolocity(velocity);

        // ?.Invoke - null-conditional: only fires the event if something subscribed.
        onShotFired?.Invoke(velocity);
    }

    // detect left mouse button click and call Fire() method
    void Update()
    {
        // Pause check
        if (GameManager.Instance != null && GameManager.Instance.isPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
}
