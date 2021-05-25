using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubesProject;

public abstract class DynamicMesh : MonoBehaviour
{
    public int gridSize;
    public Material material;


    protected float[] voxels;


    public void subtractMesh(DynamicMesh meshSubtrahend){
        int sidxMax = (gridSize-1) + (gridSize-1) * gridSize + (gridSize-1) * gridSize * gridSize;
        for (int x = 0; x < meshSubtrahend.gridSize; x++)
        {  
            for (int y = 0; y < meshSubtrahend.gridSize; y++)
            { 
                for (int z = 0; z < meshSubtrahend.gridSize; z++)
                {   
                
                    Vector3 positionSubtrahend = meshSubtrahend.transform.localPosition;
                    Vector3 positionMinuend = transform.localPosition;

                    Vector3 scale = transform.localScale;
                    
                    float fx = positionSubtrahend.x + x / (meshSubtrahend.gridSize - 1.0f) - positionMinuend.x;
                    float fy = positionSubtrahend.y + y / (meshSubtrahend.gridSize - 1.0f) - positionMinuend.y;
                    float fz = positionSubtrahend.z + z / (meshSubtrahend.gridSize - 1.0f) - positionMinuend.z;
                    
                    int sx = Mathf.RoundToInt(fx / scale.x);
                    int sy = Mathf.RoundToInt(fy / scale.y);
                    int sz = Mathf.RoundToInt(fz / scale.z);
                    
                    int idx = x + y * meshSubtrahend.gridSize + z * meshSubtrahend.gridSize * meshSubtrahend.gridSize;
                    int sidx = sx + sy * gridSize + sz * gridSize * gridSize;

                    float substrat = meshSubtrahend.voxels[idx];
                    if(sidx>=0&&sidx <=sidxMax){
                        voxels[sidx] = 1;
                    }
                }
            }
            
        }
        
    }
    public void updateMesh()
    {
        //create Mesh
        Marching marching = new MarchingCubes();
        marching.Surface = 0.0f;
        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();
        
        marching.Generate(voxels, gridSize, gridSize, gridSize, verts, indices);

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    public Vector3 getPosition(){
        return transform.localPosition;
    }

    public Vector3 getScale(){
        return transform.localScale;
    }
}
