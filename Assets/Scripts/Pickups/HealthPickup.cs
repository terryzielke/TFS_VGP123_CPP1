using UnityEngine;

public class HealthPickup : BasePickup
{
    public override void OnPickup(GameObject player)
    {
        // Increase player's health
        Debug.Log("Player picked up health!");
        GameManager.Instance.Lives = GameManager.Instance.Lives + 1;
    }
}
