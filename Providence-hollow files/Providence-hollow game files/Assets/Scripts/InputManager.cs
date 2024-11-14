using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("InputAxis")]
    public Vector2 MoveDir;
    public Vector2 LookDir;

    [Header("Script referances")]
    public PlayerMovement movement;
    public Interact PlayerInteraction;
    public StateManager PlayerStateManager;

    #region Input
    private PlayerControls Controls;
    private InputAction MoveAction;
    private InputAction LookAction;

    void Awake()
    {
        //Gets the caracterControls input map
        Controls = new PlayerControls();
    }

    //Enables all the input
    private void OnEnable()
    {
        MoveAction = Controls.Player.Move;
        MoveAction.Enable();

        LookAction = Controls.Player.Look;
        LookAction.Enable();

        Controls.Player.Jump.performed += JumpFunction;
        Controls.Player.Jump.Enable();

        Controls.Player.Interact.performed += InteractFunction;
        Controls.Player.Interact.Enable();

        Controls.Player.Run.performed += EnableRunFunction;
        Controls.Player.Run.canceled += DisableRunFunction;
        Controls.Player.Run.Enable();

        Controls.Player.Crouch.performed += EnableCrouchFunction;
        Controls.Player.Crouch.canceled += DisableCrouchFunction;
        Controls.Player.Crouch.Enable();
    }

    //Disables all the input
    private void OnDisable()
    {
        MoveAction.Disable();
        LookAction.Disable();

        Controls.Player.Jump.performed -= JumpFunction;
        Controls.Player.Jump.Disable();

        Controls.Player.Interact.performed -= InteractFunction;
        Controls.Player.Interact.Disable();

        Controls.Player.Run.performed -= EnableRunFunction;
        Controls.Player.Run.Disable();

        Controls.Player.Crouch.performed -= EnableCrouchFunction;
        Controls.Player.Crouch.Disable();
    }
#endregion

    //A jump function that gets called when pressing the jump button
    public void JumpFunction(InputAction.CallbackContext context)
    {
        movement.Jump();
    }

    //An interact function that gets called when you press the interact buttin
    public void InteractFunction(InputAction.CallbackContext context)
    {
        PlayerInteraction.interact();
    }

    //A run function that gets called whebn you press the run button
    public void EnableRunFunction(InputAction.CallbackContext context)
    {
        PlayerStateManager.StartRun();
    }

    //a function that makes the player stop running
    public void DisableRunFunction(InputAction.CallbackContext context)
    {
        PlayerStateManager.StopRun();
    }

    //A crouch function that gets called when you press the crouch button
    public void EnableCrouchFunction(InputAction.CallbackContext context)
    {
        PlayerStateManager.StartCrouching();
    }

    //Stops crouching again
    public void DisableCrouchFunction(InputAction.CallbackContext context)
    {
        PlayerStateManager.StopCrouching();
    }

    //Updates the public the public input vectors
    void FixedUpdate(){
        MoveDir = MoveAction.ReadValue<Vector2>();
        LookDir = LookAction.ReadValue<Vector2>();
    }
}
