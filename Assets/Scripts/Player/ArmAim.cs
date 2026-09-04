using UnityEngine;

/// <summary>
/// Pins an arm sprite to the correct shoulder joint based on which way the
/// player is facing, then rotates it to aim at the mouse cursor.
/// Attach to the Arm object (the one holding the arm SpriteRenderer).
/// </summary>
public class ArmAim : MonoBehaviour
{
    // [Header] is just Inspector organisation - like your #region blocks but
    // visible in the editor. New to this project; purely cosmetic.
    [Header("Shoulder anchors (children of Player)")]
    [SerializeField] private Transform armJointR;   // shoulder used when facing RIGHT
    [SerializeField] private Transform armJointL;   // shoulder used when facing LEFT

    [Header("Facing source")]
    // The BODY's SpriteRenderer. We read its flipX to know which way we face,
    // matching the logic in PlayerController.SpriteFlip.
    [SerializeField] private SpriteRenderer bodySprite;

    [Header("Options")]
    // If your arm art points straight up by default instead of to the right (+X),
    // set this to -90. Points left? 180. Lets you correct art without redrawing it.
    [SerializeField] private float spriteAngleOffset = 0f;

    // Camera.main runs a tagged object search on every call, so grab it once.
    private Camera cam;

    // This arm's own SpriteRenderer, so we can flip it when aiming left.
    private SpriteRenderer armSprite;

    private void Awake()
    {
        cam = Camera.main;
        armSprite = GetComponent<SpriteRenderer>();
    }

    // LateUpdate runs AFTER every Update() has finished this frame, so the player
    // has already moved and flipped. Positioning the arm here prevents a
    // one-frame lag where the arm visibly trails the body. New to this project -
    // PlayerController does all its work in Update().
    private void LateUpdate()
    {
        if (cam == null) cam = Camera.main;   // camera might be created after us
        if (cam == null) return;

        // --- 1. Which shoulder? ---
        // Assuming the art is drawn facing RIGHT by default:
        //   flipX == false -> facing right -> right shoulder
        //   flipX == true  -> facing left  -> left shoulder
        bool facingLeft = bodySprite != null && bodySprite.flipX;
        Transform activeJoint = facingLeft ? armJointL : armJointR;

        // --- 2. Snap to that shoulder ---
        transform.position = activeJoint.position;

        // --- 3. Cursor position in world units ---
        // Input.mousePosition is in SCREEN pixels. ScreenToWorldPoint needs a z
        // that equals the camera-to-play-plane distance; with the camera at
        // negative z and gameplay on z = 0, that distance is -camera.z.
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -cam.transform.position.z;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);

        // --- 4. Direction from shoulder to cursor ---
        Vector2 aimDir = (Vector2)(mouseWorld - transform.position);

        // --- 5. Direction -> Z angle ---
        // Atan2(y, x) returns the vector's angle in RADIANS, measured
        // counter-clockwise from +X. Rad2Deg converts to degrees because
        // Quaternion.Euler expects degrees. 2D rotation is always around Z.
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + spriteAngleOffset);

        // --- 6. Stop the arm looking upside-down when aiming left ---
        // Past +/-90 degrees the sprite is mirrored along its length, which reads
        // as "upside down". Flipping it vertically in that half keeps the
        // hand/elbow the right way up. If your art flips wrong, swap flipY -> flipX.
        if (armSprite != null)
            armSprite.flipY = aimDir.x < 0f;
    }
}