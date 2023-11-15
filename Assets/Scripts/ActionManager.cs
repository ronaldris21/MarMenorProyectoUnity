using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActionManager : MonoBehaviour
{
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public InputActionAsset controls; /// use XRIDefaultInputAcions
    public InputActionReference leftJoystickInputAction;
    public InputActionReference rightJoystickInputAction; 
    public InputActionReference leftTriggerInputAction;
    public InputActionReference rightTriggerInputAction;
    public InputActionReference leftGripInputAction;
    public InputActionReference rightGripInputAction;
    public InputActionReference leftPrimaryButtonInputAction;
    public InputActionReference rightPrimaryButtonInputAction;
    public InputActionReference leftSecondaryButtonInputAction;
    public InputActionReference rightSecondaryButtonInputAction;

    public Vector2 leftControllerMovementInput = new Vector2(0, 0);
    public Vector2 rightControllerMovementInput = new Vector2(0, 0);
    public float leftTriggerInput = 0f;
    public float rightTriggerInput = 0f;
    public float leftGripInput = 0f;
    public float rightGripInput = 0f;
    public float leftPrimaryButtonInput = 0f;
    public float rightPrimaryButtonInput = 0f;
    public float rightSecondaryButtonInput = 0f;
    public float leftSecondaryButtonInput = 0f;

    private void Awake()
    {
        leftJoystickInputAction.action.performed += context => MoveLeftController(context.ReadValue<Vector2>());
        rightJoystickInputAction.action.performed += context => MoveRightController(context.ReadValue<Vector2>());
        leftGripInputAction.action.performed += context => PressGripLeftController(context.ReadValue<float>());
        rightGripInputAction.action.performed += context => PressGripRightController(context.ReadValue<float>());
        leftTriggerInputAction.action.performed += context => PressTriggerLeftController(context.ReadValue<float>());
        rightTriggerInputAction.action.performed += context => PressTriggerRightController(context.ReadValue<float>());
        rightPrimaryButtonInputAction.action.performed += context => PressPrimaryButtonRightController(context.ReadValue<float>());
        leftPrimaryButtonInputAction.action.performed += context => PressPrimaryButtonLeftController(context.ReadValue<float>());
        rightSecondaryButtonInputAction.action.performed += context => PressSecondaryButtonRightController(context.ReadValue<float>());
        leftSecondaryButtonInputAction.action.performed += context => PressSecondaryButtonLeftController(context.ReadValue<float>());
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    void MoveLeftController(Vector2 intensity)
    {
        leftControllerMovementInput = intensity;
    }

    void PressGripLeftController(float intensity)
    {
        leftGripInput = intensity;
    }

    void MoveRightController(Vector2 intensity)
    {
        rightControllerMovementInput = intensity;
    }

    void PressTriggerLeftController(float intensity)
    {
        leftTriggerInput = intensity;
    }

    void PressTriggerRightController(float intensity)
    {
        rightTriggerInput = intensity;
    }

    void PressGripRightController(float intensity)
    {
        rightGripInput = intensity;
    }

    void PressPrimaryButtonRightController(float intensity)
    {
        rightPrimaryButtonInput = intensity;
    }

    void PressPrimaryButtonLeftController(float intensity)
    {
        leftPrimaryButtonInput = intensity;
    }

    void PressSecondaryButtonRightController(float intensity)
    {
        rightSecondaryButtonInput = intensity;
    }

    void PressSecondaryButtonLeftController(float intensity)
    {
        leftSecondaryButtonInput = intensity;
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
