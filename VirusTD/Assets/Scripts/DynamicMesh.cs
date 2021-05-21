using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubesProject;

public abstract class DynamicMesh
{
    protected float[] voxels;
    protected int gridSize;


    protected Mesh mesh;
    protected GameObject ob;

    public DynamicMesh(Vector3 position, Vector3 scale, int gridSize, Transform transform, Material material)
    {
        this.gridSize = gridSize;
    }

    public void subtractMesh(DynamicMesh meshSubtrahend){
        int sidxMax = (gridSize-1) + (gridSize-1) * gridSize + (gridSize-1) * gridSize * gridSize;
        for (int x = 0; x < meshSubtrahend.gridSize; x++)
        {  
            for (int y = 0; y < meshSubtrahend.gridSize; y++)
            { 
                for (int z = 0; z < meshSubtrahend.gridSize; z++)
                {   
                
                    Vector3 positionSubtrahend = meshSubtrahend.ob.transform.localPosition;
                    Vector3 positionMinuend = ob.transform.localPosition;

                    Vector3 scale = ob.transform.localScale;
                    
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

        mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
    public Vector3 getPosition(){
        return ob.transform.localPosition;
    }

    public Vector3 getScale(){
        return ob.transform.localScale;
    }
}
