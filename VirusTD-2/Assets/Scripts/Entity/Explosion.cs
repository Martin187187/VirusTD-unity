using System.Collections.Generic;
using UnityEngine;
public class Explosion : MonoBehaviour
{
    public TerrainManager manager;
    // Start is called before the first frame update
    void Start()
    {
        float scale = manager.getScale();
        Vector3 position = transform.position;
        int size = 5;

        HashSet<Block> blockSet = new HashSet<Block>();
        for (int i = -size / 2; i < size / 2; i += 1)
        {
            for (int j = -size / 2; j < size / 2; j += 1)
            {
                for (int z = -size / 2; z < size / 2; z += 1)
                {
                    Vector3 current = new Vector3(
                        position.x + i * scale,
                        position.y + j * scale,
                        position.z + z * scale);
                    float distance = Vector3.Distance(current, position);
                    if (distance < size / 2 * scale)
                    {
                        List<Block> blockList = manager.getAllBlocksFromPosition(current);

                        foreach (Block block in blockList)
                        {
                            block.intersect(current);
                            blockSet.Add(block);
                        }
                    }
                }
            }
        }
        foreach (Block block in blockSet)
        {
            block.updateMesh();
        }
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }



}
