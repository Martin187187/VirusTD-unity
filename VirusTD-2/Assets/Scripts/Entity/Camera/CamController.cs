using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float value = Input.GetAxis("Mouse ScrollWheel");
         if (value != 0f ){
             transform.Translate(new Vector3(0,0,value*2000*Time.deltaTime*Mathf.Sqrt(transform.localPosition.y)));
             transform.localPosition = new Vector3(0, Mathf.Max(1, transform.localPosition.y), Mathf.Min(1, transform.localPosition.z));
         }
    }
}
