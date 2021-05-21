using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform trackedObject;
    public Vector3 positionOffset = new Vector3(0, 10, -10);
    public float speed = 0.02f;

    private Vector3 oldMousePosition;

    void Start(){
        oldMousePosition = Input.mousePosition;
        transform.Translate(positionOffset);
    }
    void Update()
    {
        if(Input.GetMouseButton(2)){
            Vector3 mouseMovement = Input.mousePosition - oldMousePosition;
        transform.Translate(new Vector3(mouseMovement.x * speed * Time.deltaTime, 0, mouseMovement.y * speed * Time.deltaTime));

        }
        oldMousePosition = Input.mousePosition;

    }
}
