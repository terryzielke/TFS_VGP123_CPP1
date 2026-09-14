using System;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Projectile projectilePrefab;

    private Vector2 leftShotVelocity;
    public Action<Vector2> onShotFired;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Only the reference checks are still meaningful now.
        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("TurretShoot: assign spawnPointLeft, spawnPointRight, and projectilePrefab in the Inspector.");
        }
    }

    public void TurretFire()
    {
        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("TurretShoot: missing required references.");
            return;
        }

        // sr.flipX is set by TurretEnemy.Update() based on which side the player is on.
        // Reuse that same flag here so the projectile always matches which way the
        // turret is visually facing, instead of always firing right.
        Transform spawnPoint = sr.flipX ? spawnPointLeft : spawnPointRight;
        Vector2 dir = sr.flipX ? Vector2.left : Vector2.right;

        Vector2 velocity = dir * projectileSpeed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        Projectile curProjectile = Instantiate(projectilePrefab, spawnPoint.position, rot);
        curProjectile.SetVolocity(velocity);

        onShotFired?.Invoke(velocity);
    }

    // detect left mouse button click and call Fire() method
    void Update()
    {
        // Pause check
        if (GameManager.Instance != null && GameManager.Instance.isPaused) return;

    }
}
