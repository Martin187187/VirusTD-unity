using System;
using UnityEngine;

public class MeshBuilder
{
    private static TerrainCharacteristic characteristic;
    public static void init(TerrainCharacteristic b)
    {
        characteristic = b;
    }
    public static Tuple<float[], ColorMode[], float[]> createTerrainMesh(Vector3Int position, int gridSize, float perlinNoiseScale)
    {
        float[] voxels = new float[gridSize * gridSize * gridSize];
        float[] hps = new float[gridSize * gridSize * gridSize];
        ColorMode[] colors = new ColorMode[gridSize * gridSize * gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {



                    float fx = position.x + x / (gridSize - 1.0f);
                    float fy = position.y + y / (gridSize - 1.0f);
                    float fz = position.z + z / (gridSize - 1.0f);

                    NodeResult y2d = characteristic.GetNodeResult(fx, fz);
                    int idx = x + y * gridSize + z * gridSize * gridSize;
                    if (fy > y2d.getHeight())
                    {
                        voxels[idx] = 1;
                    }
                    else
                    {
                        //voxels[idx] = fractal.Sample3D(fx, fy, fz);
                        voxels[idx] = -1;
                    }
                    colors[idx] = y2d.GetColor();
                    hps[idx] = 1;

                }
            }

        }
        return Tuple.Create(voxels, colors, hps);
    }

    public static Tuple<float[], ColorMode[], float[]> createCubeMesh(int gridSize)
    {
        float[] voxels = new float[gridSize * gridSize * gridSize];
        ColorMode[] colors = new ColorMode[gridSize * gridSize * gridSize];
        float[] hps = new float[gridSize * gridSize * gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {

                    int idx = x + y * gridSize + z * gridSize * gridSize;
                    if (x == 0 || y == 0 || z == 0 || x == (gridSize - 1) || y == (gridSize - 1) || z == (gridSize - 1))
                        voxels[idx] = 1;
                    else
                        voxels[idx] = -1;
                    colors[idx] = ColorMode.ROCK;
                    hps[idx] = 1;
                }
            }
        }

        return Tuple.Create(voxels, colors, hps);
    }
    public static Tuple<float[], ColorMode[]> createExplosionMesh(int gridSize)
    {
        float[] voxels = new float[gridSize * gridSize * gridSize];
        ColorMode[] colors = new ColorMode[gridSize * gridSize * gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {


                    int idx = x + y * gridSize + z * gridSize * gridSize;
                    if (x == 0 || y == 0 || z == 0 || x == (gridSize - 1) || y == (gridSize - 1) || z == (gridSize - 1))
                        voxels[idx] = 1;
                    else
                        voxels[idx] = UnityEngine.Random.Range(0, 1) == 0 ? 1 : -1;
                    colors[idx] = ColorMode.ROCK;
                }
            }
        }

        return Tuple.Create(voxels, colors);
    }
}
