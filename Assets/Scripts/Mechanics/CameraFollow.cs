using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;
    [SerializeField] private Transform target;

    //private void Awake() => GameManager.Instance.OnPlayerSpawned += (player) => target = player.transform;

    // Awake() is guaranteed to run on every active object in the scene
    // before ANY object's Start() runs. That guarantee is why this has
    // to live here instead of in Start().
    //
    // LevelSpawn.Start() is what actually calls GameManager.SpawnPlayer(),
    // which fires OnPlayerSpawned. If CameraFollow subscribed inside its
    // own Start() instead, there'd be no guaranteed order between
    // CameraFollow.Start() and LevelSpawn.Start() - Unity doesn't promise
    // which runs first when they're on different GameObjects. Guess wrong
    // and CameraFollow subscribes *after* the event already fired - and
    // events don't replay past invocations, so it would miss it silently.
    
    void Awake()
    {
        // "+=" is event subscription: it adds HandlePlayerSpawned to the
        // list of methods GameManager will call when it invokes
        // OnPlayerSpawned. Any number of scripts could subscribe to the
        // same event - GameManager broadcasts without knowing who's out there.
        GameManager.Instance.OnPlayerSpawned += HandlePlayerSpawned;
    }

    // Always unsubscribe when the subscriber goes away. GameManager is a
    // DontDestroyOnLoad singleton, so it outlives this camera across scene
    // loads. Without this, GameManager keeps holding a reference to a
    // destroyed CameraFollow forever - a memory leak, and it will throw a
    // MissingReferenceException the next time OnPlayerSpawned fires.
    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned -= HandlePlayerSpawned;
        }
    }

    // Renamed from OnPlayerSpawned(Transform) -> HandlePlayerSpawned(PlayerController)
    // Renamed from OnPlayerSpawned(Transform) -> HandlePlayerSpawned(PlayerController)
    // so the signature matches PlayerInstanceDelegate exactly: that's a
    // requirement for "+=" to work at all, not just a style choice.
    private void HandlePlayerSpawned(PlayerController player)
    {
        Debug.Log($"CameraFollow received OnPlayerSpawned at {Time.time} seconds.");
        target = player.transform;
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

        // update the y position to follow the target, clamped between minYPos and maxYPos
        currentPos.y = Mathf.Clamp(target.position.y, minYPos, maxYPos);

        // apply the updated position to the camera
        transform.position = Vector3.MoveTowards(transform.position, currentPos, Time.deltaTime * 30f);
    }
}