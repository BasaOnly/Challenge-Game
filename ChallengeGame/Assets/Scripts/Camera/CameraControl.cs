using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("OffSet")]
    [SerializeField] Vector3 offSet = new Vector3(0, 1.5f, 0);

    [Header("Rotation")]
    [SerializeField] [Range(0.1f, 1f)] float rotationSensitivity;
    [SerializeField] [Range(-30f, -10f)] float minLimit;
    [SerializeField] [Range(40f, 75f)] float maxLimit;

    [Header("Position")]
    [SerializeField] [Range(0.01f, 10f)] float positionSensitivity;
    Vector3 refVelocity = Vector3.zero;

    [Header("Distance")]
    [SerializeField] float playerDistance;

    [Header("Collider")]
    [SerializeField] LayerMask colliderLayer;
    [SerializeField] float radiusCollider;
    [SerializeField] float zoomVelocity;
    [SerializeField] float smoothCollision;
    float blockDistance;
    float targetDistance;
    float refBlock;

    float x, y, mouseX, mouseY;
    Vector3 position;
    Quaternion rotation;
    Vector3 t, f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        targetDistance = playerDistance;
    }

    void Update()
    {
        UpdateInput();
        RaycastCamera();
    }

    private void LateUpdate()
    {
        if (GameManager.instance.stopActionsPlayer) { position = transform.position; return; }
        CameraMovement();
    }

    #region cameraBehaviour
    void UpdateInput()
    {
        x += (mouseX * rotationSensitivity);
        y = ClampAngle(y - mouseY * rotationSensitivity, minLimit, maxLimit);
    }
    void CameraMovement()
    {
        rotation = Quaternion.AngleAxis(x, Vector3.up) * Quaternion.AngleAxis(y, Vector3.right);
        t = target.position + rotation * offSet;
        f = rotation * -Vector3.forward;
        position = t + f * playerDistance;
        Vector3 suavizaPos = Vector3.SmoothDamp(transform.position, position, ref refVelocity, positionSensitivity * Time.deltaTime);
        transform.position = suavizaPos;
        transform.LookAt(t);
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

    #region systemInput
    public void OnMouseValues(float mouseX, float mouseY)
    {
        if (GameManager.instance.stopActionsPlayer) return;
       
        this.mouseX = mouseX;
        this.mouseY = mouseY;
    }
    #endregion
}
