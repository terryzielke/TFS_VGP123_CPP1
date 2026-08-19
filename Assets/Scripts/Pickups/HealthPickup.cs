using UnityEngine;

public class HealthPickup : BasePickup
{
    public override void OnPickup(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Increase player's health
            Debug.Log("Player picked up health!");
            playerController.Lives = playerController.Lives + 1;
        }
    }
}
