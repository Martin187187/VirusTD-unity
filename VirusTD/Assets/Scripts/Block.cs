using UnityEngine;
using System.Collections.Generic;

public class Block : DynamicMesh
{

    private float perlinNoiseScale = 0.5f;
    
    public Block(Vector3 position, Vector3 scale, int gridSize, Transform transform, Material material) :
    base(position, scale, gridSize, transform, material)
    {

        voxels = new float[gridSize * gridSize * gridSize];
        for (int x = 0; x < gridSize; x++)
        {  
            for (int y = 0; y < gridSize; y++)
            { 
                for (int z = 0; z < gridSize; z++)
                {   
                
                    
                    
                    float fx = position.x + x / (gridSize - 1.0f);
                    float fy = position.y + y / (gridSize - 1.0f);
                    float fz = position.z + z / (gridSize - 1.0f);

                    float y2d = Mathf.PerlinNoise(fx*perlinNoiseScale, fz*perlinNoiseScale)*2;
                    int idx = x + y * gridSize + z * gridSize * gridSize;

                    if(fy-2>y2d)
                        voxels[idx] = 1;
                    else
                        //voxels[idx] = fractal.Sample3D(fx, fy, fz);
                        voxels[idx] = -1;
                }
            }
            
        }

        updateMesh();

        
        ob = new GameObject("Block");

        ob.transform.localPosition = new Vector3(position.x*scale.x, position.y*scale.y, position.z*scale.z);
        ob.transform.localScale = new Vector3(scale.x/(gridSize-1), scale.y/(gridSize-1), scale.z/(gridSize-1));
        ob.transform.parent = transform;
        ob.AddComponent<MeshFilter>();
        ob.AddComponent<MeshRenderer>();
        ob.GetComponent<Renderer>().material = material;
        ob.GetComponent<MeshFilter>().mesh = mesh;
        ob.AddComponent<MeshCollider>();

    }
    public void digHole(Vector3Int start, Vector3Int end){
        for (int x = start.x; x < end.x; x++)
            {  
                for (int y = start.y; y < end.y; y++)
                { 
                    for (int z = start.z; z < end.z; z++)
                    {   
                        int idx = x + y*gridSize+z*gridSize*gridSize;
                        voxels[idx] = -1;
                    }
                }
            }
            updateMesh();
            ob.GetComponent<MeshFilter>().mesh = mesh;
    }

    
    public Mesh GetMesh()
    {
        return mesh;
    }

    public GameObject GetGameObject()
    {
        return ob;
    }

}
