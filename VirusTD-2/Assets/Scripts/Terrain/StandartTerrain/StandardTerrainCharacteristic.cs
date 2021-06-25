using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardTerrainCharacteristic : TerrainCharacteristic
{

    public Color color = new Color(0.6f,0.29f,0);
    public float middleHeight = 0, min = 2, max = 2;
    public int steps = 4;
    public int offset;

    public override NodeResult GetNodeResult(float x, float z){

        float height = Mathf.PerlinNoise(x*scale + offset, z*scale +offset);

        float minScaled = (height-1)*min+1;
        float maxScaled = minScaled*max;
        float heightCut = Mathf.Min(1, Mathf.Max(0, maxScaled));
        
        float invStep = 1/(float)steps;
        float heightStep = (int)(heightCut/invStep)*invStep;
        return new NodeResult(heightStep * amplitude + middleHeight, color);
    }

    public override List<TerrainCharacteristic> GetTerrainCharacteristics(){
        List<TerrainCharacteristic> result = new List<TerrainCharacteristic>();
        return result;
    }
}
