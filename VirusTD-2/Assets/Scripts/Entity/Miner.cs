using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Entity
{
    public float shootFrequenz = 5f;
    private float rotX = 0;
    private float rotY = 0;
    
    
    private float timer;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {   
        //bool doshoot = Input.GetKeyDown(KeyCode.P);
        bool doshoot = timer/shootFrequenz>1;

        if(doshoot){
            //rdm direction
            rotX = Random.Range(-70, -0);
            rotY = Random.Range(-20, 20);

        }

        Transform arm = transform.GetChild(0);
        Transform laser = arm.GetChild(0);

        Quaternion newRotArm = Quaternion.Euler(0,rotY,0);
        arm.localRotation = Quaternion.Slerp(arm.localRotation, newRotArm,  Time.deltaTime * 5);

        Quaternion newRotLaser = Quaternion.Euler(rotX, 0, 0);
        laser.localRotation = Quaternion.Slerp(laser.localRotation, newRotLaser,  Time.deltaTime * 5);

        if(doshoot){
            Debug.Log("shoot at: " +laser.rotation * Vector3.down);
            Debug.DrawRay(laser.position, (laser.rotation  * Vector3.down)*100, Color.green, shootFrequenz, false);
            if(world!=null)
                shoot(laser.position, laser.rotation * Vector3.down);
            timer = 0;
        }
        timer+=Time.deltaTime;
        
    }

    public void shoot(Vector3 position, Vector3 direction){
        Vector3 pos = new Vector3(position.x, position.y, position.z);
        Block block = world.getBlockFromPosition(pos);
        int i = 0;
        while(!block.intersect(pos)&&i<100){
            pos+=(direction*0.2f);
            i++;
            block = world.getBlockFromPosition(pos);
        }
        block.updateMesh();
    }


}
