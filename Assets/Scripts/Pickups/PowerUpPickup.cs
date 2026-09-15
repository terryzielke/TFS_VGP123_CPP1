using UnityEngine;

public class PointPickup : BasePickup
{
    public override void OnPickup(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Activate a power-up for the player
            Debug.Log("Player picked up points!");
            // Implement power-up activation logic here
            PlaybackRequest.Instance.RequestOneShotSound(PlaybackRequest.Instance.pointSound);

            // Add points to the player's score
            GameManager.Instance.Points += 10;

        }
    }
}
