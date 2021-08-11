using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CharacterController controler;
   
    [Header("Animation")]
    public Animator animPlayer;
    [SerializeField] float smoothAnimation;
    [SerializeField] bool run;
    float animMovementValue;

    [Header("Movimento")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed = 0.2f;
    float velocity;
    Vector3 gravity;
    bool canMove;

    [Header("CheckGround")]
    [SerializeField] Transform positionCheck;
    [SerializeField] float radiusSphereCheck;
    [SerializeField] LayerMask maskCheck;
    [SerializeField] bool onGround;

    Vector3 move;
    [HideInInspector] public Vector3 direction;
    float horizontal;
    float vertical;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        GetAnimVariables();
        CheckGround();
        if (canMove)
        {
            SimpleMovement();
        }
  
        MoveAnimator();
    }

    #region movement
    void SimpleMovement()
    {
        LocomotionRotation();
        Move();
    }

    void Move()
    {
        velocity = movementSpeed * GetInputVector().magnitude;
        velocity = run ? velocity * 1.5f : velocity;
        move = direction * velocity;
        controler.Move((GravityCalculation() + move) * Time.deltaTime);
    }

    Vector3 GravityCalculation()
    {
        if (onGround && gravity.y < 0)
            gravity.y = -2f;

        gravity.y += Physics.gravity.y * Time.deltaTime;
        return gravity;
    }

    void CheckGround()
    {
        onGround = Physics.CheckSphere(positionCheck.position, radiusSphereCheck, maskCheck);
    }

    void LocomotionRotation()
    {
        if (GetInputVector() == Vector3.zero) return; 

        direction = cam.transform.TransformDirection(GetInputVector());
        direction.y = 0;
        transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * rotationSpeed);
    }
  
    Vector3 GetInputVector()
    {
       return new Vector3(vertical, 0, horizontal).normalized;
    }

    #endregion

    #region animation
    void MoveAnimator()
    {
        animMovementValue = Mathf.Clamp(Mathf.Lerp(animMovementValue, velocity, Time.deltaTime * smoothAnimation), 0, 9);
        animPlayer.SetFloat("speed", animMovementValue);
    }

    void GetAnimVariables()
    {
        canMove = animPlayer.GetBool("canMove");
    }
    #endregion

    #region inputSystem
    public void OnMoveInput(float vertical, float horizontal)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
    }

    public void OnJump()
    {
        Debug.Log("Jump");
    }

    public void OnRun(bool value)
    {
        run = value;
    }

    #endregion

    #region gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(positionCheck.position, radiusSphereCheck);
    }
    #endregion
}
