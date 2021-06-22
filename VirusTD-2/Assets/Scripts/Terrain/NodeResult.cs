using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeResult
{
    private float height;
    private Color color;

    public NodeResult(float height, Color color){
        this.height = height;
        this.color = color;
    }



    public float getHeight(){
        return this.height;
    }

    public Color GetColor(){
        return this.color;
    }
}
