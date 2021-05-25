using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MarchingCubesProject;
public class Projectile : DynamicMesh
{
    public void Start() 
    {
        
        voxels = MeshBuilder.createCubeMesh(gridSize);
        
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = material;
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshCollider>();
        updateMesh();
        
    }


}
