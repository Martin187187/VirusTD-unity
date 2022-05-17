using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LaserLine : Turret
{

    public GameObject otherPart;
    GameObject laser;
    // Start is called before the first frame update
    float timer = 0;
    void Start()
    {
        damage = 20;
        laser = GameObject.Instantiate(Resources.Load<GameObject>("VFXGraphs/vfxgraph_beam"), transform.position, Quaternion.identity);
        laser.transform.parent = transform;
        VisualEffect ve = laser.GetComponent<VisualEffect>();

        Vector3 direction = otherPart.transform.position - transform.position;
        laser.transform.LookAt(otherPart.transform.position);
        ve.SetFloat("length", direction.magnitude / 6);
        ve.SetFloat("duration", 10f);
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 height = world.getHeight(transform.position);
        Vector3 height2 = world.getHeight(otherPart.transform.position);

        if(height.y<transform.position.y||height2.y<otherPart.transform.position.y){
            delete();
        }
        


        
        if (timer > 10)
        {
            timer = 0;
            Vector3 direction = otherPart.transform.position - transform.position;
            GameObject light = GameObject.Instantiate(Resources.Load<GameObject>("VFXGraphs/Light"), transform.position, Quaternion.identity);
            light.transform.parent = laser.transform;
            light.transform.rotation = laser.transform.rotation;
            LightMovement lightmove = light.GetComponent<LightMovement>();
            lightmove.length = direction.magnitude;
            lightmove.velocity = new Vector3(0, 0, 0.1f);
        }
        timer++;
        
    }
}
