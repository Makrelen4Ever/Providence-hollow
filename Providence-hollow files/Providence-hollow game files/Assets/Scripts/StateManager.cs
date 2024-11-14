using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private PlayerMovement Movement;
    [SerializeField] private string State;

    private Vector2 InputAxis;

    [Header("Collision")]
    [SerializeField] private float CollisionDistance;

    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private Transform GroundCheckTransform;

    [Header("Stamina system")]
    [SerializeField] private float Stamina;
    [SerializeField] private float MaxStamina;
    [Range(0, 1)]
    [SerializeField] private float TiredPercent;

    [Space]

    [SerializeField] private float StaminaGain;
    [SerializeField] private float StaminaWaste;

    [Header("Bools")]
    [SerializeField] private bool CanRun;
    [SerializeField] private bool IsTired;
    [SerializeField] private bool IsRunning;
    [SerializeField] private bool IsCrouching;

    [Space]
    
    [SerializeField] private bool RunKeyPressed;
    [SerializeField] private bool CrouchKeyPressed;

    [Header("Input")]
    [SerializeField] private InputManager inputManager;

    [Header("Stamina bar")]
    [SerializeField] private GameObject StaminaBar;
    [SerializeField] private Color CanRunColor;
    [SerializeField] private Color CannotRunColor;
    
    private bool ColorEnabled = true;

    void Update()
    {
        //Check if the player is grounded
        bool IsGrounded = Physics.CheckSphere(GroundCheckTransform.position, CollisionDistance, GroundMask);

        //Gets the input axis
        InputAxis = inputManager.MoveDir;

        //Checks if the player gets tired so they cant run for a while
        if(Stamina <= 0){
            IsTired = true;
        }else if(Stamina >= MaxStamina * TiredPercent){
            IsTired = false;
        }

        //Updates the stamina amount
        CanRun = Stamina > 0 && !IsTired;
        StaminaSystem();

        //Sets the states
        if(IsGrounded){
            //Checks if the player is grounded
            if(Mathf.Abs(InputAxis.magnitude) > 0){
                //Checks for key input to see if the player is moving
                if(RunKeyPressed && CanRun){
                    //Checks if the player is running
                    State = "Running";
                }else if(CrouchKeyPressed){
                    //Checks if the player is crouching
                    State = "Crouching";
                }else{
                    //Just walks normal if the player isnt running
                    State = "Walking";
                }
            }else{
                //If its touching the ground but not moving it sets the state to idle
                State = "Idle";
            }
        }else{
            //sets the state to falling if the player isnt touching the ground
            State = "Falling";
        }

        //Sets the bools based on the states
        switch(State){
            case "Running":
                IsRunning = true;
                IsCrouching = false;
                break;

            case "Walking":
                IsRunning = false;
                IsCrouching = false;
                break;

            case "idle":
                IsRunning = false;
                IsCrouching = false;
                break;

            case "falling":
                IsRunning = false;
                IsCrouching = false;
                break;

            case "Crouching":
                IsRunning = false;
                IsCrouching = true;
                break;
        }

        //Transfers the local bools to the player bools
        Movement.IsRunning = IsRunning;
        Movement.IsCrouching = IsCrouching;
        Movement.IsGrounded = IsGrounded;
    }

    void StaminaSystem(){
        //Checks if the runkey is pressed and increase/decrease the stamina
        IsRunning = RunKeyPressed && Stamina > 0 && CanRun;
        Stamina += IsRunning ? StaminaWaste * Time.deltaTime : StaminaGain * Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);

        // sets the scale and color of the stamina bar in the corner
        StaminaBar.transform.localScale = new Vector3(Stamina/MaxStamina, 1, 1);
        if(ColorEnabled){StaminaBar.GetComponent<Image>().color = CanRun ? CanRunColor : CannotRunColor;}
    }
    
    //Starts to run
    public void StartRun(){
        //Disables crouching when starting to run
        CrouchKeyPressed = false;
        RunKeyPressed = true;
    }

    public void StopRun(){
        //Disables crouching when starting to run
        RunKeyPressed = false;
    }

    //Starting to crouch
    public void StartCrouching(){
        //Disables Running when starting to crouch
        IsRunning = false;
        CrouchKeyPressed = true;
    }

    public void StopCrouching(){
        //Disables Running when starting to crouch
        CrouchKeyPressed = false;
    }
}
