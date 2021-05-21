using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float speed = 20f;
    private Vector3 oldMousePosition;
    // Start is called before the first frame update
    void Start()
    {
        oldMousePosition = Input.mousePosition;
        
    }

    // Update is called once per frame
    void Update()
    {
         
        if(Input.GetMouseButton(2)){
            Vector3 mouseMovement = Input.mousePosition - oldMousePosition;
        transform.Translate(new Vector3(mouseMovement.x * speed * Time.deltaTime, 0, mouseMovement.y * speed * Time.deltaTime));

        }
        
        oldMousePosition = Input.mousePosition;
    }
}
