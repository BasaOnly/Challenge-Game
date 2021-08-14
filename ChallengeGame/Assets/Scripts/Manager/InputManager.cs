using UnityEngine;
using UnityEngine.InputSystem;

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
        playerActions.PlayerControls.Attack.performed += OnAttack;
        playerActions.PlayerControls.SkillChange.performed += OnSkillChange;
        playerActions.PlayerControls.Interaction.performed += OnInteraction;
        playerActions.PlayerControls.Run.performed += OnRun;
        playerActions.PlayerControls.Run.canceled += OnRun;
        playerActions.PlayerControls.Defend.performed += OnDefend;
        playerActions.PlayerControls.Defend.canceled += OnDefend;
        playerActions.PlayerControls.Movement.performed += OnMovement;
        playerActions.PlayerControls.Movement.canceled += OnMovement;
        playerActions.PlayerControls.Camera.performed += OnCameraRotation;
        playerActions.PlayerControls.Camera.canceled += OnCameraRotation;

    }


    #region inputSystem
    //Axis
    public void OnMovement(InputAction.CallbackContext value)
    {
        UIManager.instance.ChangeNameKey(value.action.activeControl.shortDisplayName);
        Vector2 inputMovement = value.ReadValue<Vector2>();
        vectorEvent[0].Invoke(inputMovement.y, inputMovement.x);
    }
    public void OnCameraRotation(InputAction.CallbackContext value)
    {
        Vector2 inputMouseRotation = value.ReadValue<Vector2>();
        vectorEvent[1].Invoke(inputMouseRotation.x, inputMouseRotation.y);
    }

    //Trigger
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (GameManager.instance.stopActionsPlayer) return;
        triggerEvents[0].Invoke();
    }

    public void OnSkillChange(InputAction.CallbackContext value)
    {
        if (GameManager.instance.stopActionsPlayer) return;
        triggerEvents[1].Invoke();
    }

    public void OnInteraction(InputAction.CallbackContext value)
    {
 
    
        triggerEvents[2].Invoke();
    }

    //Bool
    public void OnRun(InputAction.CallbackContext value)
    {
        if (GameManager.instance.stopActionsPlayer) return;
        triggerBoolEvents[0].Invoke(value.performed);
    }

    public void OnDefend(InputAction.CallbackContext value)
    {
        if (GameManager.instance.stopActionsPlayer) return;
        triggerBoolEvents[1].Invoke(value.performed);
    }

    #endregion
}


