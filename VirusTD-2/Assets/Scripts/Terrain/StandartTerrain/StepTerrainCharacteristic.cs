using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTerrainCharacteristic : TerrainCharacteristic
{
    public Color color = new Color(1,1,1);
    public float middleHeight = 0, min = 2, max = 2;
    public float curve = 1;

    public List<TerrainCharacteristic> terrainLevel = new List<TerrainCharacteristic>();


    
    public override NodeResult GetNodeResult(float x, float z){

        float height = Mathf.PerlinNoise(x*scale, z*scale);

        float minScaled = (height-1)*min+1;
        float maxScaled = minScaled*max;
        float heightCut = Mathf.Pow(Mathf.Min(1, Mathf.Max(0, maxScaled)), curve);
        
        float invStep = 1/(float)(terrainLevel.Count-1);
        int floor = (int)(heightCut/invStep);
        float heightStep = floor*invStep;


        if(floor> (terrainLevel.Count-1) || floor < 0) throw new System.Exception("number: " + floor);
        float endHeight = heightStep * amplitude + middleHeight;

        float secondHeight = terrainLevel[floor].GetNodeResult(x,z).getHeight()+amplitude*invStep*floor;
        return new NodeResult(Mathf.Max(endHeight, secondHeight), color);
    }

    public override List<TerrainCharacteristic> GetTerrainCharacteristics(){
        List<TerrainCharacteristic> result = new List<TerrainCharacteristic>();
        return result;
    }
}
