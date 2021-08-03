using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Entity
{
    public float shootFrequenz = 1f;
    private float rotX = 0;
    private float rotY = 0;
    
    
    private float timer;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {   

        if(timer/shootFrequenz>1){
            //rdm direction
            rotX = Random.Range(-70, -0);
            rotY = Random.Range(-20, 20);

        }

        Transform arm = transform.GetChild(0);
        Transform laser = arm.GetChild(0);

        Quaternion newRotArm = Quaternion.Euler(0,rotY,0);
        arm.localRotation = Quaternion.Slerp(arm.rotation, newRotArm,  Time.deltaTime * 5);

        Quaternion newRotLaser = Quaternion.Euler(rotX, 0, 0);
        laser.localRotation = Quaternion.Slerp(laser.localRotation, newRotLaser,  Time.deltaTime * 5);

        if(timer/shootFrequenz>1){
            Debug.Log("shoot at: " +laser.rotation * Vector3.down);
            if(world!=null)
                shoot(laser.position, laser.rotation * Vector3.down);
            timer = 0;
        }
        timer+=Time.deltaTime;
        
    }

    public void shoot(Vector3 position, Vector3 direction){
        Vector3 pos = new Vector3(position.x, position.y, position.z);
        List<Block> blockList = world.getAllIntersectingBlockPosition(pos);
        int i = 0;
        while(!doIntersect(blockList, pos)&&i<100){
            pos+=direction;
            i++;
            blockList = world.getAllIntersectingBlockPosition(pos);
        }
    }
    private bool doIntersect(List<Block> blocks, Vector3 pos){
        bool inter = false;
        foreach(Block block in blocks){
            bool current = block.intersect(pos);
            inter |= current;

            if(current)
                block.updateMesh();
        }
        return inter;
    }


}
