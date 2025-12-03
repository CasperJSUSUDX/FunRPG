using UnityEngine;

public class playerModel : MonoBehaviour
{
    // // Assign this in the Inspector or it'll try to auto-find a child named "playerModel"
    // public Transform modelTransform;
    // [SerializeField] private bool facingRight = true;

    // // Default rotations
    // private static readonly Vector3 DefaultRotation = new Vector3(0f, 180f, 0f);
    // private static readonly Vector3 RightRotation = new Vector3(0f, 110f, 0f);
    // private static readonly Vector3 LeftRotation = new Vector3(0f, 250f, 0f);

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         FaceLeft();
    //     }

    //     if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         FaceRight();
    //     }
    // }

    // public void FaceLeft()
    // {
    //     if (modelTransform == null) return;
    //     modelTransform.localEulerAngles = LeftRotation;
    //     facingRight = false;
    // }

    // public void FaceRight()
    // {
    //     if (modelTransform == null) return;
    //     modelTransform.localEulerAngles = RightRotation;
    //     facingRight = true;
    // }
    // [ContextMenu("Flip")]
    // private void Flip()
    // {
    //     if (modelTransform == null) return;
    //     if (facingRight) FaceLeft(); else FaceRight();
    // }
    public Transform modelTransform;

    public Vector3 defaultLocalEuler = new Vector3(0f, 180f, 0f);

    public float smoothTime = 0.2f;

    // If you prefer a constant degrees-per-second turning, use RotateTowards in Update instead.
    // public float rotationSpeedDegPerSec = 360f;

    private float yVelocity = 0f;
    private bool initialized = false;

    private void Start()
    {
        // Auto-find modelTransform if not assigned
        if (modelTransform == null)
        {
            modelTransform = transform.Find("playerModel");
            if (modelTransform == null && transform.childCount > 0)
                modelTransform = transform.GetChild(0);
        }

        if (modelTransform == null)
        {
            Debug.LogWarning($"[PlayerModel] modelTransform not assigned and no child found on '{gameObject.name}'. Please assign it in the Inspector.");
            enabled = false;
            return;
        }

        // Set default rotation on start
        modelTransform.localEulerAngles = defaultLocalEuler;
        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        // Read keys (works with legacy Input; if using new Input System, adapt accordingly)
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        float targetY;

        // Compute direction vector on XZ plane where:
        // z = forward/back (W positive), x = right/left (D positive)
        float x = (right ? 1f : 0f) + (left ? -1f : 0f);
        float z = (up    ? 1f : 0f) + (down ? -1f : 0f);

        if (Mathf.Approximately(x, 0f) && Mathf.Approximately(z, 0f))
        {
            // No input: use default idle rotation
            targetY = NormalizeAngle(defaultLocalEuler.y);
        }
        else
        {
            // Atan2(x, z) gives angle relative to +Z (forward) which maps to the requested angles:
            // (x=0,z=1) -> 0°, (x=1,z=0) -> 90°, (x=0,z=-1) -> 180°, (x=-1,z=0) -> -90/270°
            float angle = Mathf.Atan2(x, z) * Mathf.Rad2Deg;
            targetY = NormalizeAngle(angle);
        }

        // Smoothly damp the current Y towards targetY (handles wrap-around correctly)
        float currentY = NormalizeAngle(modelTransform.localEulerAngles.y);
        float newY = Mathf.SmoothDampAngle(currentY, targetY, ref yVelocity, smoothTime);
        modelTransform.localEulerAngles = new Vector3(0f, newY, 0f);

        // If you prefer RotateTowards (constant speed) instead, uncomment below and comment out the SmoothDampAngle lines above:
        // Quaternion currentRot = modelTransform.localRotation;
        // Quaternion targetRot = Quaternion.Euler(0f, targetY, 0f);
        // modelTransform.localRotation = Quaternion.RotateTowards(currentRot, targetRot, rotationSpeedDegPerSec * Time.deltaTime);
    }

    // Normalize angle to [0, 360)
    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    // Optional helper methods you can call from other scripts
    public void SnapToDefault()
    {
        if (modelTransform == null) return;
        modelTransform.localEulerAngles = defaultLocalEuler;
        yVelocity = 0f;
    }

    public void SnapToAngle(float yDegrees)
    {
        if (modelTransform == null) return;
        modelTransform.localEulerAngles = new Vector3(0f, NormalizeAngle(yDegrees), 0f);
        yVelocity = 0f;
    }

}