using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{

    public float length;
    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        if(pos.z>length){
            Destroy(gameObject);
        }
        transform.localPosition+=velocity;
    }
}
