using UnityEngine;
using System;
using UnityEditor;

public class Block : DynamicMesh
{
    public Vector3Int index;
    public int[,] heightMap;

    public void Start()
    {

        gameObject.layer = 3;
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshCollider>();
        updateMesh();

        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().materials = materials;


        //AssetDatabase.CreateAsset(GetComponent<MeshFilter>().mesh, "Assets/Resources/test" + index.x +"-"+ index.y +"-"+ index.z +"-"+ ".asset");
        //AssetDatabase.SaveAssets();



    }

    public void rebuild()
    {


        Tuple<float[], ColorMode[], float[]> results = MeshBuilder.createTerrainMesh(index, gridSize, 1f);
        voxels = results.Item1;
        colorList = results.Item2;
        hp = results.Item3;
        updateMesh();
    }
    public void digHole(Vector3Int start, Vector3Int end)
    {

        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                for (int z = start.z; z < end.z; z++)
                {
                    int idx = x + y * gridSize + z * gridSize * gridSize;

                    if (!(start.x == x && x > 0 || start.y == y && y > 0 || start.z == z && z > 0))
                        voxels[idx] = -1;
                    colorList[idx] = ColorMode.STEEL;
                }
            }
        }
        updateMesh();

    }

    public void fill(Vector3Int start, Vector3Int end)
    {
        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                for (int z = start.z; z < end.z; z++)
                {
                    int idx = x + y * gridSize + z * gridSize * gridSize;

                    if (!(start.x == x && x > 0 || start.y == y && y > 0 || start.z == z && z > 0))
                    {
                        if (colorList[idx] == ColorMode.STEEL)
                            hp[idx] -= 0.1f;
                        else
                            hp[idx] -= 1f;
                        if (hp[idx] <= 0)
                        {
                            voxels[idx] = 1;
                        }
                    }

                }
            }
        }
        updateMesh();

    }

    public override void specificUpdate()
    {
        calculateHeightMap();
    }

    public void calculateHeightMap()
    {
        int[,] result = new int[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                for (int y = gridSize - 1; y >= 0; y--)
                {
                    int idx = x + y * gridSize + z * gridSize * gridSize;
                    if (voxels[idx] < 0)
                    {
                        result[x, z] = y;
                        break;
                    }
                }
            }
        }
        heightMap = result;
    }


}
