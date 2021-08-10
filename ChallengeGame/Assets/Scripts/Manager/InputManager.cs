using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public VectorEvent[] vectorEvent;
    public OnTriggerEvent[] triggerEvents;
    PlayerActions playerActions;
 

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);

        playerActions = new PlayerActions();
        playerActions.Enable();

        playerActions.PlayerControls.Jump.performed += OnJump;
        playerActions.PlayerControls.Attack.performed += OnAttack;
        playerActions.PlayerControls.Movement.performed += OnMovement;
        playerActions.PlayerControls.Movement.canceled += OnMovement;
        playerActions.PlayerControls.Camera.performed += OnCameraRotation;
        playerActions.PlayerControls.Camera.canceled += OnCameraRotation;

    }

    #region inputSystem
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        vectorEvent[0].Invoke(inputMovement.x, inputMovement.y);
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        triggerEvents[0].Invoke();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        triggerEvents[1].Invoke();
    }

    public void OnCameraRotation(InputAction.CallbackContext value)
    {
        Vector2 inputMouseRotation = value.ReadValue<Vector2>();
        vectorEvent[1].Invoke(inputMouseRotation.x, inputMouseRotation.y);
    }

    #endregion
}

//Evemts
[Serializable]
public class VectorEvent : UnityEvent<float, float> { }

[Serializable]
public class OnTriggerEvent : UnityEvent { }
