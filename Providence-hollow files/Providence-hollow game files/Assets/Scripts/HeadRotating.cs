using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotating : MonoBehaviour
{
    [Header("Rotating")]
    public float Xsens;
    public float Ysens;

    private Vector2 InputAxis;
    
    private float Xrot;
    private float Yrot;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        InputAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        Yrot = InputAxis.x * Xsens;
        Xrot += InputAxis.y * -Ysens;
        Xrot = Mathf.Clamp(Xrot, -80, 80);

        transform.localRotation = Quaternion.Euler(Xrot, 0, 0);
        transform.parent.Rotate(new Vector3(0, Yrot, 0));
    }
}
