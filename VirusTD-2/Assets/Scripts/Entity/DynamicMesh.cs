using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubesProject;
public enum TextureMode
{
    Beton = 0, 
    Crushed = 1
}
public abstract class DynamicMesh : MonoBehaviour
{
    public int gridSize;
    public Material material;


    protected float[] voxels;


    public void subtractMesh(DynamicMesh meshSubtrahend){
        int sidxMax = (gridSize-1) + (gridSize-1) * gridSize + (gridSize-1) * (gridSize-1) * (gridSize-1);
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

                    Debug.Log("aa: " + sidx);
                    float substrat = meshSubtrahend.voxels[idx];
                    if(sidx>=0&&sidx <=sidxMax){
                        voxels[sidx] = 1;
                    }
                }
            }
            
        }
        
    }

    public bool intersect(DynamicMesh subtrahend)
    {
        int sidxMax = (gridSize-1) + (gridSize-1) * gridSize + (gridSize-1) * (gridSize-1) * (gridSize-1);
        Vector3 positionSubtrahend = subtrahend.transform.localPosition;
        Vector3 positionMinuend = transform.localPosition;

        Vector3 scale = transform.localScale;
        
        float fx = positionSubtrahend.x - positionMinuend.x;
        float fy = positionSubtrahend.y - positionMinuend.y;
        float fz = positionSubtrahend.z - positionMinuend.z;
        
        int sx = Mathf.RoundToInt(fx / scale.x);
        int sy = Mathf.RoundToInt(fy / scale.y);
        int sz = Mathf.RoundToInt(fz / scale.z);
        
        int sidx = sx + sy * gridSize + sz * gridSize * gridSize;
        if(sidx>=0&&sidx <= sidxMax){
            if(voxels[sidx]<1){
                voxels[sidx] = 1;
                return true;
            }
            
        }
        return false;
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

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        
        Vector2[] uvs = new Vector2[vertices.Length];
        Color[] colors = new Color[vertices.Length];
        float max = 0;
        for (int i = 0; i < uvs.Length; i++)
        {   
            if(normals[i].x > 0)
                uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
            else if(normals[i].x < 0)
                uvs[i] = new Vector2(gridSize - 1 - vertices[i].z, vertices[i].y);
            else if(normals[i].z > 0)
                uvs[i] = new Vector2(gridSize - 1 - vertices[i].x, vertices[i].y);
            else if(normals[i].z < 0)
                uvs[i] = new Vector2(vertices[i].x,vertices[i].y);
            else if(normals[i].y > 0)
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            else if(normals[i].y < 0)
                uvs[i] = new Vector2(vertices[i].x, gridSize - 1 - vertices[i].z);
            else
                uvs[i] = new Vector2(vertices[i].x,vertices[i].y);


            
            colors[i] = new Color(1, 1, 1);

            if(vertices[i].x > max)
                max = vertices[i].x;
        }

        Debug.Log("max: " + max);
        mesh.uv = uvs;
        mesh.colors = colors;

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        if(this is Block){
            gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
        }
    }
    public Vector3 getPosition(){
        return transform.localPosition;
    }

    public Vector3 getScale(){
        return transform.localScale;
    }
}
