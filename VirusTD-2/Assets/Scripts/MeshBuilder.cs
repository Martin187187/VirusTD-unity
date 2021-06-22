using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder 
{   
    private static TerrainCharacteristic characteristic;
    public static void init(TerrainCharacteristic b){
        characteristic = b;
    }
    public static float[] createTerrainMesh(Vector3Int position, int gridSize, float perlinNoiseScale){
        float[] voxels = new float[gridSize * gridSize * gridSize];
        for (int x = 0; x < gridSize; x++)
        {  
            for (int y = 0; y < gridSize; y++)
            { 
                for (int z = 0; z < gridSize; z++)
                {   
                
                    
                    
                    float fx = position.x + x / (gridSize - 1.0f);
                    float fy = position.y + y / (gridSize - 1.0f);
                    float fz = position.z + z / (gridSize - 1.0f);

                    float y2d = characteristic.GetNodeResult(fx, fz).getHeight();
                    int idx = x + y * gridSize + z * gridSize * gridSize;
                    if(fy>y2d) {
                        voxels[idx] = 1;
                    } else {
                        //voxels[idx] = fractal.Sample3D(fx, fy, fz);
                        voxels[idx] = -1;
                    }
                }
            }
            
        }
        return voxels;
    }

    public static float[] createCubeMesh(int gridSize) {
        float[] voxels = new float[gridSize * gridSize * gridSize];
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

        return voxels;
    }
}
