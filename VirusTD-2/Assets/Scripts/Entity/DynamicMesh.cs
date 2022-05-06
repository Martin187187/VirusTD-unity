using System;
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
    public Material[] materials;


    public float[] voxels;
    public ColorMode[] colorList;


    public void subtractMesh(DynamicMesh meshSubtrahend)
    {
        int sidxMax = gridSize + gridSize * gridSize + gridSize * gridSize * gridSize;
        for (int x = 0; x <= meshSubtrahend.gridSize; x++)
        {
            for (int y = 0; y <= meshSubtrahend.gridSize; y++)
            {
                for (int z = 0; z <= meshSubtrahend.gridSize; z++)
                {

                    Vector3 positionSubtrahend = meshSubtrahend.transform.localPosition;
                    Vector3 positionMinuend = transform.localPosition;

                    Vector3 scale = transform.localScale;
                    Vector3 scaleSubtrahend = meshSubtrahend.transform.localScale;
                    float fx = positionSubtrahend.x + x / (meshSubtrahend.gridSize - 1.0f)*scaleSubtrahend.x - positionMinuend.x;
                    float fy = positionSubtrahend.y + y / (meshSubtrahend.gridSize - 1.0f)*scaleSubtrahend.y - positionMinuend.y;
                    float fz = positionSubtrahend.z + z / (meshSubtrahend.gridSize - 1.0f)*scaleSubtrahend.z - positionMinuend.z;

                    int sx = Mathf.RoundToInt(fx / scale.x);
                    int sy = Mathf.RoundToInt(fy / scale.y);
                    int sz = Mathf.RoundToInt(fz / scale.z);

                    int idx = x + y * meshSubtrahend.gridSize + z * meshSubtrahend.gridSize * meshSubtrahend.gridSize;
                    int sidx = sx + sy * gridSize + sz * gridSize * gridSize;

                    Debug.Log("aa: " + sidx);
                    float substrat = meshSubtrahend.voxels[idx];
                    if (sidx >= 0 && sidx <= sidxMax)
                    {
                        voxels[sidx] = 1;
                        colorList[sidx] = ColorMode.ROCK;
                    }
                }
            }

        }

    }

    public bool intersect(Vector3 pos)
    {
        int sidxMax = voxels.Length;
        Vector3 positionSubtrahend = pos;
        Vector3 positionMinuend = transform.localPosition;

        Vector3 scale = transform.localScale;

        float fx = positionSubtrahend.x - positionMinuend.x;
        float fy = positionSubtrahend.y - positionMinuend.y;
        float fz = positionSubtrahend.z - positionMinuend.z;
        int sx = Mathf.RoundToInt(fx / scale.x);
        int sy = Mathf.RoundToInt(fy / scale.y);
        int sz = Mathf.RoundToInt(fz / scale.z);

        Debug.DrawRay(transform.position+new Vector3(sx*scale.x, sy*scale.y, sz*scale.z), Vector3.up, Color.cyan, 10);
        int sidx = sx + sy * gridSize + sz * gridSize * gridSize;
        if (sidx >= 0 && sidx <= sidxMax)
        {
            try{

           
            if (voxels[sidx] < 1)
            {

                voxels[sidx] = 1;
                return true;
            }
             }catch(Exception e){
                Debug.Log("error: "+ e+", max: "+sidxMax + ", acc: "+sidx);
            }

        }
        //GameObject gay = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //gay.transform.position = pos;
        //gay.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        return false;
    }
    public abstract void specificUpdate();
    public void updateMesh()
    {
        //create Mesh
        Marching marching = new MarchingCubes();
        marching.Surface = 0.0f;
        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();
        List<ColorMode> colors = new List<ColorMode>();

        marching.Generate(voxels, colorList, gridSize, gridSize, gridSize, verts, indices, colors);

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        Vector2[] uvs = new Vector2[vertices.Length];
        Color[] colors2 = new Color[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            if (normals[i].x > 0)
                uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
            else if (normals[i].x < 0)
                uvs[i] = new Vector2(gridSize - 1 - vertices[i].z, vertices[i].y);
            else if (normals[i].z > 0)
                uvs[i] = new Vector2(gridSize - 1 - vertices[i].x, vertices[i].y);
            else if (normals[i].z < 0)
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
            else if (normals[i].y > 0)
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            else if (normals[i].y < 0)
                uvs[i] = new Vector2(vertices[i].x, gridSize - 1 - vertices[i].z);
            else
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
            if (colors[i] == ColorMode.STEEL)
                colors2[i] = new Color(0.25f, 0, 0);
            else if (colors[i] == ColorMode.ROCK)
                colors2[i] = new Color(0, 0, 0);
            else if (colors[i] == ColorMode.HIGHGRASS && normals[i].y > 0)
                colors2[i] = new Color(0.15f, 1, 0);
            else if (normals[i].y > 0)
                colors2[i] = new Color(0.15f, 0, 0);
            else
                colors2[i] = new Color(0, 0, 0);


        }


        mesh.uv = uvs;
        mesh.colors = colors2;

            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            
            if (this is Block)
            {
                gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
            }
        specificUpdate();
    }
    public Vector3 getPosition()
    {
        return transform.localPosition;
    }

    public Vector3 getScale()
    {
        return transform.localScale;
    }
}
