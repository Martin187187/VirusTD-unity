using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public Entity goal;
    private List<Block> path = new List<Block>();

    public void Update(){
        if(path.Count>0){
            move();
        } else if(goal!=null){
            Block startBlock = world.getBlockFromPosition(transform.position);
            Block endBlock = world.getBlockFromPosition(goal.transform.position);
            path = BlockStrategy.calculatePath(world, startBlock, endBlock);

            if(path.Count==0){
                path.Add(startBlock);
                Debug.Log("lool");
            }
        }
    }
    
    public void move(){
        if(path.Count == 0){
            goal = null;
            return;
        }
        Block currentBlock = path[0];
        transform.LookAt(currentBlock.transform);
        if(Vector3.Distance(transform.position, currentBlock.transform.position)< 0.1f)
            path.Remove(currentBlock);
        transform.Translate(0,0,0.1f);

    }

}
