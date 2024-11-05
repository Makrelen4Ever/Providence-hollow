using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private PlayerMovement Movement;
    [SerializeField] private string State;

    private Vector2 inputAxis;

    [Header("Collision")]
    [SerializeField] private float CollisionDistance;

    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private Transform GroundCheckTransform;

    [Header("Stamina system")]
    [SerializeField] private float Stamina;
    [SerializeField] private float MaxStamina;

    [Space]

    [SerializeField] private float StaminaGain;
    [SerializeField] private float StaminaWaste;

    [Header("Bools")]
    [SerializeField] private bool CanRun;
    [SerializeField] private bool IsTired;
    [SerializeField] private bool IsRunning;

    [Space]

    [SerializeField] private bool IsCrouching;

    [Header("KeyCodes")]
    [SerializeField] private KeyCode RunKey;
    [SerializeField] private KeyCode CrouchKey;

    void Update()
    {
        //Check if the player is grounded
        bool IsGrounded = Physics.CheckSphere(GroundCheckTransform.position, CollisionDistance, GroundMask);

        //Gets the input axis
        inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Checks if the player gets tired so they cant run for a while
        if(Stamina <= 0){
            IsTired = true;
        }else if(Stamina >= MaxStamina * 0.1f){
            IsTired = false;
        }

        //Updates the stamina amount
        CanRun = Stamina > 0 && !IsTired;
        StaminaSystem();

        //Sets the states
        if(IsGrounded){
            //Checks if the player is grounded
            if(Mathf.Abs(inputAxis.magnitude) > 0){
                //Checks for key input to see if the player is moving
                if(Input.GetKey(RunKey) && CanRun){
                    //Checks if the player is running
                    State = "Running";
                }else if(Input.GetKey(CrouchKey)){
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
    }

    void StaminaSystem(){
        //Checks if the runkey is pressed and increase/decrease the stamina
        IsRunning = Input.GetKey(RunKey) && Stamina > 0 && CanRun;
        Stamina += IsRunning ? StaminaWaste * Time.deltaTime : StaminaGain * Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);
    }
}
