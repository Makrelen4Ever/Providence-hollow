using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Anim")]
    public bool IsOpen;
    public bool IsOpening;
    [SerializeField] private AnimationCurve AnimCurve;

    //Opens the door
    public IEnumerator OpenDoor(float speedInSeconds, float steps){
        IsOpening = true;
        float Evaluation = 0;
        for(var i = 0; i <= steps; i++){
            Evaluation += 1/steps;
            transform.localRotation = Quaternion.Euler(0, AnimCurve.Evaluate(Evaluation) * 90, 0);
            yield return new WaitForSeconds(speedInSeconds / steps); 
        }
        IsOpening = false;
    }

    //Closes the door
    public IEnumerator CloseDoor(float speedInSeconds, float steps){
        IsOpening = true;
        float Evaluation = 1;
        for(var i = 0; i <= steps; i++){
            Evaluation -= 1/steps;
            Debug.Log(Evaluation);
            transform.localRotation = Quaternion.Euler(0, AnimCurve.Evaluate(Evaluation) * 90, 0);
            yield return new WaitForSeconds(speedInSeconds / steps); 
        }
        IsOpening = false;
    }
}
