using UnityEngine;
using System;
using UnityEditor;

public class Block : DynamicMesh
{
    public Vector3Int index;

    public void Start(){

        Tuple<float[], ColorMode[]> results = MeshBuilder.createTerrainMesh(index, gridSize, 1f);
        voxels = results.Item1;
        colorList = results.Item2;

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshCollider>();
        updateMesh();
        
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().materials = materials;
        
        
        //AssetDatabase.CreateAsset(GetComponent<MeshFilter>().mesh, "Assets/Resources/test" + index.x +"-"+ index.y +"-"+ index.z +"-"+ ".asset");
        //AssetDatabase.SaveAssets();
        
        

    }

    public void rebuild(){
        
        Tuple<float[], ColorMode[]> results = MeshBuilder.createTerrainMesh(index, gridSize, 1f);
        voxels = results.Item1;
        colorList = results.Item2;
        updateMesh();
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
                        colorList[idx] = ColorMode.ROCK;
                    }
                }
            }
            updateMesh();
            
    }
 
    

}
