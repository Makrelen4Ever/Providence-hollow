using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField] private float ArmLength;
    [SerializeField] private LayerMask InteractableMask;


    [Header("Opening/Closing doors")]
    [SerializeField] private float TimeInSeconds;
    [SerializeField] private float Steps;
    
    
    public void interact()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, ArmLength, InteractableMask);

        if(hit.collider.CompareTag("Door")){
            Door DoorScript = hit.collider.gameObject.GetComponent<Door>();
            if(DoorScript.IsOpening){
                return;
            }

            if(DoorScript.IsOpen){
                StartCoroutine(DoorScript.CloseDoor(TimeInSeconds, Steps));
            }else{
                StartCoroutine(DoorScript.OpenDoor(TimeInSeconds, Steps));
            }

            DoorScript.IsOpen = !DoorScript.IsOpen;
        }
    }
}
