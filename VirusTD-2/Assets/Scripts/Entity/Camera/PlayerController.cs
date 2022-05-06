using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float panSpeed = 50f;
    public Transform cameraTransform; 
    public TerrainManager terrainManager;
    private Vector3 lastMousePosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            float speed = cameraTransform.localPosition.y;
            Vector3 delta = lastMousePosition-Input.mousePosition;
            transform.Translate(delta.x * speed * Time.deltaTime, 0, delta.y * speed * Time.deltaTime);
            transform.localPosition = terrainManager.getHeight(transform.localPosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 rotation = new Vector3(0, delta.x, 0);
            transform.Rotate(rotation * Time.deltaTime * panSpeed, Space.Self);
        } 
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            lastMousePosition = Input.mousePosition;
        }
    }
}
