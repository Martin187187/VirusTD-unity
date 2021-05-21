using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform trackedObject;
    public Transform trackedObject2;

    public float mouseSensitivity = 100f;
    public float mouseWheelSensitivity = 200f;

    public float mouseWheelMin = 0f;
    public float mouseWheelMax = 100f;

    private float xRotation = 0f;
    private float distance = 0f;

    void Update()
    {
        if(Input.GetMouseButton(1)){
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
            xRotation -= mouseY;
            
            trackedObject.Rotate(Vector3.up * mouseX);
            trackedObject2.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        }
        distance += Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity * Time.deltaTime* 100;
        transform.localPosition = new Vector3(0,0, distance);

    }
}
