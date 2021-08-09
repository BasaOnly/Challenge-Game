using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("OffSet")]
    [SerializeField] Vector3 offSet = new Vector3(0, 1.5f, 0);

    [Header("Rotation")]
    [SerializeField] [Range(1f, 6f)] float rotationSensitivity;
    [SerializeField] [Range(-30f, -10f)] float minLimit;
    [SerializeField] [Range(40f, 75f)] float maxLimit;
    [SerializeField] float smoothSpeedRotPos;

    [Header("Distance")]
    [SerializeField] float playerDistance;
    [SerializeField] float zoom;
    [SerializeField] float minDistance, maxDistance;

    [Header("Collider")]
    [SerializeField] LayerMask colliderLayer;
    [SerializeField] float radiusCollider;
    [SerializeField] float zoomVelocity;
    [SerializeField] float smoothCollision;
    float blockDistance;
    float targetDistance;
    float refBlock;

    float x, y;
    Vector3 position;
    Quaternion rotation;
    Vector3 t, f;

    void Start()
    {
        targetDistance = playerDistance;
    }

    void Update()
    {
        UpdateInput();
        RaycastCamera();
        CameraMovement();
    }

    #region cameraBehaviour
    void UpdateInput()
    {
        x += (Input.GetAxis("Mouse X") * rotationSensitivity);
        y = ClampAngle(y - Input.GetAxis("Mouse Y") * rotationSensitivity, minLimit, maxLimit);

        targetDistance = Mathf.Clamp(targetDistance + ZoomAdd, minDistance, maxDistance);
    }

    void CameraMovement()
    {
        rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);

        t = target.position + rotation * offSet;
        f = rotation * -Vector3.forward;
        position = t + f * playerDistance;

        Vector3 smoothPosition = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothSpeedRotPos);
        transform.position = smoothPosition;

        Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothSpeedRotPos);
        transform.rotation = smoothRotation;
    }

    void RaycastCamera()
    {
        playerDistance += (targetDistance - playerDistance) * zoomVelocity * Time.deltaTime;

        if (Physics.SphereCast(t, radiusCollider, f, out RaycastHit hit, targetDistance, colliderLayer))
            blockDistance = Mathf.SmoothDamp(blockDistance, hit.distance, ref refBlock, smoothCollision * Time.deltaTime);
        else 
            blockDistance = targetDistance;

        playerDistance = Mathf.Min(playerDistance, blockDistance);
    }
    #endregion

    #region utility
    float ZoomAdd
    {
        get
        {
            float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
            if (scrollAxis > 0) return -zoom;
            if (scrollAxis < 0) return zoom;
            return 0;
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        return Mathf.Clamp(angle, min, max);
    }
    #endregion

    #region gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(t, f * targetDistance);
        Gizmos.DrawSphere(transform.position, radiusCollider);
    }
    #endregion
}
