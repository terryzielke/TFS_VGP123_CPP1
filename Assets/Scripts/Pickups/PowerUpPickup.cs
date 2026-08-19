using UnityEngine;

public class PowerUpPickup : BasePickup
{
    public override void OnPickup(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Activate a power-up for the player
            Debug.Log("Player picked up power-up!");
            // Implement power-up activation logic here
        }
    }
}
