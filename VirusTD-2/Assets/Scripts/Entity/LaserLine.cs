using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LaserLine : Turret
{

    public GameObject otherPart;
    // Start is called before the first frame update
    void Start()
    {
        GameObject en = GameObject.Instantiate(Resources.Load<GameObject>("VFXGraphs/vfxgraph_beam"), transform.position , Quaternion.identity);
        en.transform.parent = transform;
        VisualEffect ve = en.GetComponent<VisualEffect>();
        
        Vector3 direction = otherPart.transform.position -transform.position;
        en.transform.LookAt(otherPart.transform.position);
        ve.SetFloat("length", direction.magnitude/6);
        ve.SetFloat("duration", 100f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
