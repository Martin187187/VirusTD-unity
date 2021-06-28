using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform trackedObject;

    public float mouseSensitivity = 100f;


    private float xRotation = 0f;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
            xRotation -= mouseY;
            Mathf.Clamp(xRotation, -90f, 90f);
            
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            trackedObject.Rotate(Vector3.up * mouseX); 


    }
    
}
