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

    public KeyCode RunKey = KeyCode.LeftShift;

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
    public LayerMask GroundMask;
    public Transform GroundCheckTransform;

    private bool IsGrounded;

    private CharacterController CC;

    void Start(){
        CC = GetComponent<CharacterController>();
    }

    void Update(){

        //Ground check
        GroundCheckTransform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z); // Sets the scale of the groundcheck object to 1 
        IsGrounded = Physics.CheckSphere(GroundCheckTransform.position, CollisionDistance, GroundMask);   // checks if the player is grounded


        //Gravity
        if(IsGrounded && GravityVelocity.y < 0){
            GravityVelocity.y = 0;
        }

        GravityVelocity.y -= Gravity * Time.deltaTime;
        CC.Move(GravityVelocity * Time.deltaTime);

        //Check if running
        IsRunning = Input.GetKey(RunKey) && Stamina > 0 && CanRun;
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