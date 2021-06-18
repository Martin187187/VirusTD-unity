using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class Block : DynamicMesh
{
    public Vector3Int index;

    public void Start(){

        voxels = MeshBuilder.createTerrainMesh(index, gridSize, 0.5f);
        
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = material;
        gameObject.AddComponent<MeshCollider>();

        updateMesh();
        
        
        //AssetDatabase.CreateAsset(GetComponent<MeshFilter>().mesh, "Assets/Resources/test" + index.y + ".asset");
        //AssetDatabase.SaveAssets();
        
        

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
            
    }
 
    

}
