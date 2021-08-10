using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CharacterController controler;

    [Header("Movimento")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed = 0.2f;

    Vector3 move;
    Vector3 direction;
    float horizontal;
    float vertical;
    Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        LocomotionRotation();
        Move();
    }

    void Move()
    {
        move = direction * movementSpeed * GetInputVector().magnitude;
        controler.Move(move * Time.deltaTime);
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
    #region inputSystem
    public void OnMoveInput(float vertical, float horizontal)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
    }

    public void OnAttack()
    {
        Debug.Log("Ataque");
    }

    public void OnJump()
    {
        Debug.Log("Jump");
    }

    #endregion
}
