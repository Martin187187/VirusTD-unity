using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockConcreteFactory
{
        public Block ConstructEntity(Vector3Int indexPosition, Transform transform, int length, int gridSize){
            GameObject ob = new GameObject("Block");
            ob.transform.localPosition = new Vector3(indexPosition.x*length, indexPosition.y*length, indexPosition.z*length);
            ob.transform.localScale = new Vector3(length/(gridSize-1f), length/(gridSize-1f), length/(gridSize-1f));
            ob.transform.parent = transform;
            Block b = ob.AddComponent<Block>();
            b.index = new Vector3Int(indexPosition.x,indexPosition.y,indexPosition.z);
            b.gridSize = gridSize;
            b.materials = new Material[2];
            b.materials[0] = Resources.Load("Material", typeof(Material)) as Material;
            b.materials[1] = Resources.Load("wheat", typeof(Material)) as Material;
            return b;
        }
}
