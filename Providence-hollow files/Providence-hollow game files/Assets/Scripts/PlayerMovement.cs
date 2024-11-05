using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

#region Input

private Vector2 InputAxis;

void UpdateInput(){
    InputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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

    void Jump(){
        if(Input.GetKeyDown("space") && IsGrounded){
            GravityVelocity.y = JumpForce;
        }
    }

#endregion

#region Gravity And Collision
    [Header("Gravity")]
    [SerializeField] private float Gravity;

    private Vector3 GravityVelocity;

    [Header("Collision")]
    [SerializeField] private float CollisionDistance;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private Transform GroundCheckTransform;

    private bool IsGrounded;

    private CharacterController CC;

    void Grav(){
        //Ground check
        GroundCheckTransform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z); // Sets the scale of the groundcheck object to 1 
        IsGrounded = Physics.CheckSphere(GroundCheckTransform.position, CollisionDistance, GroundMask);   // checks if the player is grounded


        //Gravity
        if(IsGrounded && GravityVelocity.y < 0){
            GravityVelocity.y = 0;
        }

        GravityVelocity.y -= Gravity * Time.fixedDeltaTime;
        CC.Move(GravityVelocity * Time.fixedDeltaTime);
    }

#endregion

    void Start(){
        CC = GetComponent<CharacterController>();
    }

    void Update(){
        UpdateInput();
        Jump();
    }

    void FixedUpdate()
    {
        Grav();
        Move();
    }
}
