using UnityEngine;
using System.Collections.Generic;

public class PickupSpawner : MonoBehaviour
{
    [System.Serializable]
    private struct PickupSpawnEntry
    {
        public GameObject pickupPrefab;
        public Transform spawnPoint;
    }
    [SerializeField] private List<PickupSpawnEntry> pickupSpawns;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (pickupSpawns.Count > 0)
        {
            // Generate a random index to select a pickup spawn entry
            int randomIndex = Random.Range(0, pickupSpawns.Count);
            PickupSpawnEntry randomEntry = pickupSpawns[randomIndex];
            // Check if the spawn point is null before instantiating the pickup
            if (randomEntry.spawnPoint != null && randomEntry.pickupPrefab != null)
            {
                // Instantiate the pickup prefab at the spawn point's position and rotation
                Instantiate(randomEntry.pickupPrefab, randomEntry.spawnPoint.position, randomEntry.spawnPoint.rotation);
            }
        }
    }
}
