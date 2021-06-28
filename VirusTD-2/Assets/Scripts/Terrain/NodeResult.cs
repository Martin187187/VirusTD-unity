using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeResult
{
    private float height;
    private ColorMode colorMode;



    public NodeResult(float height, ColorMode colorMode){
        this.height = height;
        this.colorMode = colorMode;
    }



    public float getHeight(){
        return this.height;
    }

    public ColorMode GetColor(){
        return this.colorMode;
    }

}
