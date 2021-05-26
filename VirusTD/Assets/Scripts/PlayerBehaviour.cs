using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform po;
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
        if(po!=null){
            transform.localPosition = po.localPosition;
        } else {
        if(Input.GetMouseButton(2)){
            Vector3 mouseMovement = Input.mousePosition - oldMousePosition;
        transform.Translate(new Vector3(mouseMovement.x * speed * Time.deltaTime, 0, mouseMovement.y * speed * Time.deltaTime));

        }
        if(Input.GetKey(KeyCode.W)){
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.S)){
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.A)){
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        if(Input.GetKey(KeyCode.D)){
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        if(Input.GetKey(KeyCode.Space)){
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        if(Input.GetKey(KeyCode.LeftControl)){
            transform.Translate(0, - speed * Time.deltaTime, 0);
        }
        
        oldMousePosition = Input.mousePosition;
        }
    }
}
