using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotating : MonoBehaviour
{
#region Input
    [Header("Inputs")]
    [SerializeField] private InputManager inputManager;

    private Vector2 InputAxis;

    //Gets the input vector from the input manager
    void UpdateInput(){
        InputAxis = inputManager.LookDir;
    }

#endregion

    [Header("Rotating")]
    [SerializeField] private float Xsens;
    [SerializeField] private float Ysens;
    
    private float Xrot;
    private float Yrot;

    //Locks the cursor
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        //Updates the input
        UpdateInput();

        //updates the rotation vectors
        Yrot = InputAxis.x * Xsens;
        Xrot += InputAxis.y * -Ysens;
        Xrot = Mathf.Clamp(Xrot, -80, 80);

        //Updates the rotation based on the rotation vectors
        transform.localRotation = Quaternion.Euler(Xrot, 0, 0);
        transform.parent.Rotate(new Vector3(0, Yrot, 0));
    }
}
