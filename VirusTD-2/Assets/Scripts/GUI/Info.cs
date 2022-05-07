using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{

    public TerrainManager world;
    void OnGUI()
    {
        GUI.Box(new Rect(10,10,100,150), "Wave: "+world.waveCounter);

        
        if(GUI.Button(new Rect(20,40,80,20), world.running ? "Stop" : "Unpause"))
        {
            world.running = !world.running;
        }
        GUI.Label(new Rect(20,70,80,20), "Gold: "+world.gold);
        GUI.Label(new Rect(20,100,80,20), "HP: "+world.turret.hp);
    }


}
