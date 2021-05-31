using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MarchingCubesProject;
public class Projectile : DynamicMesh
{
    public Vector3 velocity;
    public void Start() 
    {
        
        voxels = MeshBuilder.createCubeMesh(gridSize);
        
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = material;
        gameObject.AddComponent<MeshFilter>();


        updateMesh();
        
    }

    public void Update() {
        {
            
            transform.localPosition = transform.localPosition + velocity*0.1f;
        }
    }

}
