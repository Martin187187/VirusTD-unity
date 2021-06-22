using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainCharacteristic : MonoBehaviour
{
    public TerrainManager terrainManager;
    public float scale = 0.4f, amplitude = 1;
    // Start is called before the first frame update
    
    private bool ignoreFirstValidate = false;
    public void OnValidate(){
        if(Application.isPlaying && ignoreFirstValidate){
            terrainManager.reloadTerrain();
        }
        else
        {
            ignoreFirstValidate = true;
        }
    }
    public abstract NodeResult GetNodeResult(float x, float z);
    public abstract List<TerrainCharacteristic> GetTerrainCharacteristics();
}
