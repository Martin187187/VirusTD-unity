using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStrategy
{


    public static List<Block> calculatePath(TerrainManager world, Block startBlock, Block endBlock){
        List<Block> path = new List<Block>();    

        Vector3Int startBlockPosition = startBlock.index;
        Vector3Int endBlockPosition = endBlock.index;

        int i = startBlockPosition.x, j = startBlockPosition.y, k = startBlockPosition.z;
        
                    
        while(i != endBlockPosition.x){
            if(i < endBlockPosition.x) i++;
            else i--;
            Block b = world.getBlockFromIndex(new Vector3Int(i,j,k));
            path.Add(b);
        }
        while(j != endBlockPosition.y){
            if(j < endBlockPosition.y) j++;
            else j--;
            Block b = world.getBlockFromIndex(new Vector3Int(i,j,k));
            path.Add(b);
        }
        while(k != endBlockPosition.z){
            if(k < endBlockPosition.z) k++;
            else  k--;
            Block b = world.getBlockFromIndex(new Vector3Int(i,j,k));
            path.Add(b);
        }
                    
                    
                   
        
    return path;
    }
}
