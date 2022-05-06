using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float borderDown = -45f;
    public float borderUp = 45f;

    public bool rotateY = true;

    private Vector3 lastMousePosition;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            transform.Rotate(new Vector3(delta.y, 0, 0) * Time.deltaTime * panSpeed, Space.Self);
            transform.localEulerAngles = new Vector3(ClampAngle(transform.localEulerAngles.x, borderDown, borderUp),0 ,0);
            lastMousePosition = Input.mousePosition;
        } 
    }
    private float ClampAngle(float angle,float min, float max) {
 
     if (angle<90 || angle>270){       // if angle in the critic region...
         if (angle>180) angle -= 360;  // convert all angles to -180..+180
         if (max>180) max -= 360;
         if (min>180) min -= 360;
     }    
     angle = Mathf.Clamp(angle, min, max);
     if (angle<0) angle += 360;  // if angle negative, convert to 0..360
     return angle;
 }
}
