using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public VectorEvent[] vectorEvent;
    public OnTriggerEvent[] triggerEvents;
    public OnTriggerBoolEvent[] triggerBoolEvents;
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
        playerActions.PlayerControls.SkillChange.performed += OnSkillChange;
        playerActions.PlayerControls.Run.performed += OnRun;
        playerActions.PlayerControls.Run.canceled += OnRun;
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
    public void OnCameraRotation(InputAction.CallbackContext value)
    {
        Vector2 inputMouseRotation = value.ReadValue<Vector2>();
        vectorEvent[1].Invoke(inputMouseRotation.x, inputMouseRotation.y);
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        triggerEvents[0].Invoke();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        triggerEvents[1].Invoke();
    }

    public void OnSkillChange(InputAction.CallbackContext value)
    {
        triggerEvents[2].Invoke();
    }
    public void OnRun(InputAction.CallbackContext value)
    {
        triggerBoolEvents[0].Invoke(value.performed);
    }


    #endregion
}


