using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

#region Input
[Header("Inputs")]
[SerializeField] private InputManager inputManager;

private Vector2 InputAxis;

//Getting the movement axis as a vector2 from the inpumanager
void UpdateInput(){
    InputAxis = inputManager.MoveDir;
}

#endregion

#region  Movement And Jumping

    [Header("MoveSpeed")]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float CrouchSpeed;

    [Space]

    public bool IsRunning;
    public bool IsCrouching;

    [Header("Jumping")]
    [SerializeField] private float JumpForce;

    //A function that moves the player
    void Move(){
        Vector3 vel = transform.forward * InputAxis.y + transform.right * InputAxis.x;
        if(IsRunning){
            CC.Move(vel * (RunSpeed * Time.fixedDeltaTime));
        }else if(IsCrouching){
            CC.Move(vel * (CrouchSpeed * Time.fixedDeltaTime));
        }else{
            CC.Move(vel * (WalkSpeed * Time.fixedDeltaTime));
        }
    }

    //A adds force upwards to jump
    public void Jump(){
        GravityVelocity.y = JumpForce;
    }

#endregion

#region Gravity And Collision
    [Header("Gravity")]
    [SerializeField] private float Gravity;

    [HideInInspector] public bool IsGrounded;
    
    private Vector3 GravityVelocity;
    private CharacterController CC;

    void Grav(){
        //Gravity
        if(IsGrounded && GravityVelocity.y < 0){
            GravityVelocity.y = 0;
        }

        //Moves the player  cuz of gravity
        GravityVelocity.y -= Gravity * Time.fixedDeltaTime;
        CC.Move(GravityVelocity * Time.fixedDeltaTime);
    }

#endregion

    void Start(){
        //Gets the Character controller from the player
        CC = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        //Calls the different functions that moves the player
        UpdateInput();
        Grav();
        Move();
    }
}
