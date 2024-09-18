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
        IsGrounded = Physics.CheckSphere(GroundCheckTransform.position, CollisionDistance, GroundMask);

        //Gravity
        if(IsGrounded && GravityVelocity.y < 0){
            GravityVelocity.y = 0;
        }

        GravityVelocity.y -= Gravity * Time.deltaTime;
        CC.Move(GravityVelocity * Time.deltaTime);

        //Moving
        IsRunning = Input.GetKey(KeyCode.LeftShift) && Stamina > 0 && CanRun;
        Stamina += IsRunning ? StaminaWaste * Time.deltaTime : StaminaGain * Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);

        if(Stamina <= 0){
            CanRun = false;
        }else{
            if(Stamina >= MaxStamina/2){
                CanRun = true;
            }
        }

        StaminaBar.transform.localScale = new Vector3(Stamina/MaxStamina, 1, 1);
        if(ColorEnabled){StaminaBar.GetComponent<Image>().color = CanRun ? CanRunColor : CannotRunColor;}

        InputAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MovementAxis = InputAxis.x * transform.right + InputAxis.z * transform.forward;
    
        CC.Move(MovementAxis * (IsRunning ? RunSpeed : WalkSpeed) * Time.deltaTime);

        //Jumping
        if(Input.GetKeyDown("space") && IsGrounded && CanRun && Stamina >= JumpStaminaUsage){
            GravityVelocity = new Vector3(GravityVelocity.x, JumpForce, GravityVelocity.z);
            Stamina -= JumpStaminaUsage;
        }else{
            if(Input.GetKeyDown("space") && Stamina < JumpStaminaUsage){
                StartCoroutine(ColorFlash());
            }   
        }
    }

    IEnumerator ColorFlash(){
        ColorEnabled = false;
        StaminaBar.GetComponent<Image>().color = CannotRunColor;
        yield return new WaitForSeconds(.05f);
        ColorEnabled = true;
    }
}