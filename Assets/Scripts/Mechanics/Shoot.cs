using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Vector2 initShotVolocity = new Vector2(5, 5);
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Projectile projectilePrefab;

    private Vector2 leftShotVelocity;
    public Action<Vector2> onShotFired;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if(initShotVolocity == Vector2.zero)
        {
            initShotVolocity = new Vector2(5, 5);
            Debug.LogWarning("Shoot: Initial shot velocity was zero. Defaulting to (5, 5).");
        }

        if(spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("Shoot: One or more required references are not assigned. Please ensure spawnPointLeft, spawnPointRight, and projectilePrefab are assigned in the inspector.");
        }

        leftShotVelocity = new Vector2(-initShotVolocity.x, initShotVolocity.y);
    }

    // Update is called once per frame
    public void Fire()
    {

        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("File will not work because shoot script is missing required references.");
            return;
        }

        Projectile curProjectile;

        if(!sr.flipX)
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, Quaternion.identity);
            curProjectile.SetVolocity(initShotVolocity);
            onShotFired?.Invoke(initShotVolocity);
        }
        else
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, Quaternion.identity);
            curProjectile.SetVolocity(leftShotVelocity);
            onShotFired?.Invoke(leftShotVelocity);
        }
    }
}
