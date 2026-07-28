using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    [SerializeField] private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if(player == null)
            {
                Debug.LogError("No target assigned and no GameObject tagged as player. Please ensure a reference is assigned.");
                return;
            }

            target = player.transform;
        }
    }

    // Inputs being pulled in update - Physics updates being pulled in FixedUpdate - Camera follow being pulled in LateUpdate

    // Update is with the computer tick rate - good for inputs

    // FixedUpdate is a fixed rate - good for physics updates

    // LateUpdate happens after all updates - good for camera follow

    // Update is called once per frame
    void Update()
    {
        // early return - if no target is assigned, we can't follow anything
        if (target == null) return;

        // store current position
        Vector3 currentPos = transform.position;

        // update the x position to follow the target, clamped between minXPos and maxXPos
        currentPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);

        // apply the updated position to the camera
        transform.position = Vector3.MoveTowards(transform.position, currentPos, Time.deltaTime * 5f);
    }
}
