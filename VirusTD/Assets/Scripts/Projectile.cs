using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MarchingCubesProject;
public class Projectile : DynamicMesh
{

    public Projectile(Vector3 position, Vector3 scale, int gridSize, Transform transform, Material material) :
    base(position, scale, gridSize, transform, material)
    {
        voxels = new float[gridSize * gridSize * gridSize];
        for (int x = 0; x < gridSize; x++)
        {  
            for (int y = 0; y < gridSize; y++)
            { 
                for (int z = 0; z < gridSize; z++)
                {   
                
                    int idx = x + y * gridSize + z * gridSize * gridSize;
                    if(x==0||y==0||z==0||x==(gridSize-1)||y==(gridSize-1)||z==(gridSize-1))
                        voxels[idx] = 1;
                    else
                        voxels[idx] = -1;
                }
            }
        }
           
        
        updateMesh();

        
        ob = new GameObject("Block");

        ob.transform.localPosition = new Vector3(position.x, position.y, position.z);
        ob.transform.localScale = new Vector3(scale.x/(gridSize-1), scale.y/(gridSize-1), scale.z/(gridSize-1));
        ob.transform.parent = transform;
        ob.AddComponent<MeshFilter>();
        ob.AddComponent<MeshRenderer>();
        ob.GetComponent<Renderer>().material = material;
        ob.GetComponent<MeshFilter>().mesh = mesh;
        ob.AddComponent<BoxCollider>();
    }


}
