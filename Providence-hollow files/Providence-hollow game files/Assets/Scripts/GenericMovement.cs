using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class GenericMovement : MonoBehaviour
{
    [Header("Moving")]
    public float WalkSpeed;

    [Space]
    public bool CanRun;
    public float RunSpeed;

    private bool IsRunning;

    [Header("Stamina system")]
    public float Stamina;
    public float MaxStamina;

    [Space]

    public float TiredWalkSpeed;
    public float MaxTiredWalkSpeed;

    [Space]

    public float StaminaGain;
    public float StaminaWaste;

    [Space]

    public GameObject StaminaBar;
    public Color CanRunColor;
    public Color CannotRunColor;
    
    private bool ColorEnabled = true;

    private Vector3 InputAxis;
    private Vector3 MovementAxis;

    [Header("Jumping")]
    public float JumpForce;
    public float JumpStaminaUsage;

    [Header("Crouching")]
    public float CrouchHeight;
    public float CrouchSpeed;
    public float CrouchMaxSpeed;

    [Space]

    public KeyCode CrouchKey;

    private bool IsCrouching;

    [Header("Gravity")]
    public float Gravity;

    private Vector3 GravityVelocity;

    [Header("Collision")]
    public float CollisionDistance;
    public float CollisionDistanceCrouching;
    public LayerMask GroundMask;
    public Transform GroundCheckTransform;
    public Transform GroundCheckTransformCrouching;

    private bool IsGrounded;

    private CharacterController CC;

    void Start(){
        CC = GetComponent<CharacterController>();
    }

    void Update(){

        //Ground check
        IsGrounded = IsCrouching ? Physics.CheckSphere(GroundCheckTransformCrouching.position, CollisionDistanceCrouching, GroundMask) : Physics.CheckSphere(GroundCheckTransform.position, CollisionDistance, GroundMask);


        //Gravity
        if(IsGrounded && GravityVelocity.y < 0){
            GravityVelocity.y = 0;
        }

        GravityVelocity.y -= Gravity * Time.deltaTime;
        CC.Move(GravityVelocity * Time.deltaTime);

        //Check if crouching
        if(!IsRunning && IsGrounded && Input.GetKeyDown(CrouchKey)){
            IsCrouching = !IsCrouching;
            // if(IsCrouching){
            //     Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 2, GroundMask);
            //     transform.position = hit.point + new Vector3(0, 1, 0);
            // }else{
            //     for(var i = 0; i <= 60; i++){
            //         if(IsCrouching){
            //             break;
            //         }
            //         transform.position -= new Vector3(0, 0.025f, 0);
            //     }
            //     transform.position += new Vector3(0, 0.025f, 0);
            // }
        }

        //Check if running
        IsRunning = Input.GetKey(KeyCode.LeftShift) && Stamina > 0 && CanRun;
        Stamina += IsRunning ? StaminaWaste * Time.deltaTime : StaminaGain * Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);

        //Checks if the player have enough stamina
        if(Stamina <= 0){
            CanRun = false;
        }else{
            if(Stamina >= MaxStamina/2){
                CanRun = true;
            }
        }

        // sets the scale of the stamina bar in the corner
        StaminaBar.transform.localScale = new Vector3(Stamina/MaxStamina, 1, 1);
        if(ColorEnabled){StaminaBar.GetComponent<Image>().color = CanRun ? CanRunColor : CannotRunColor;}

        //Getting move directions
        InputAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MovementAxis = InputAxis.x * transform.right + InputAxis.z * transform.forward;

        //Starts moving
        if(IsCrouching){
            CC.Move(MovementAxis * CrouchSpeed * Time.deltaTime);
            transform.localScale = new Vector3(1, CrouchHeight, 1);

        }else{
            transform.localScale = new Vector3(1, 1, 1);
            CC.Move(MovementAxis * (IsRunning ? RunSpeed : WalkSpeed) * Time.deltaTime);
        }

        //Jumping
        if(Input.GetKeyDown("space") && IsGrounded && CanRun && Stamina >= JumpStaminaUsage){
            IsCrouching = false;
            GravityVelocity = new Vector3(GravityVelocity.x, JumpForce, GravityVelocity.z);
            Stamina -= JumpStaminaUsage;
        }else{
            if(Input.GetKeyDown("space") && Stamina < JumpStaminaUsage){
                StartCoroutine(ColorFlash());
            }   
        }
    }

    IEnumerator ColorFlash(){
        //Flash the stamina bar if you dont have enough stamina
        ColorEnabled = false;
        StaminaBar.GetComponent<Image>().color = CannotRunColor;
        yield return new WaitForSeconds(.05f);
        ColorEnabled = true;
    }
}