using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        PowerUp
    }

    [SerializeField] private PickupType pickupType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                // Handle the pickup based on its type
                switch (pickupType)
                {
                    case PickupType.Health:
                        // Increase player's health
                        print("Player picked up health!");
                        player.Lives = player.Lives + 1;

                        break;
                    case PickupType.PowerUp:
                        // Activate a power-up for the player
                        print("Player picked up power-up!");
                        break;
                }
            }
            // Destroy this pickup game object from the scene
            Destroy(gameObject);
        }
    }
}