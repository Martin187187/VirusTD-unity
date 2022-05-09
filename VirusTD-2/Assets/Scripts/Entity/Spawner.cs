using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public TerrainManager world;
    float timer = 0;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (world.running ||counter> 0)
        {
            timer += 0.01f;
            if (timer > 1)
            {
                timer = 0;
                counter++;

                Vector3 spawnerPosition = transform.position;
                float scaleX = Random.Range(0, 100) / 100f * transform.localScale.x - transform.localScale.x / 2;
                float scaleZ = Random.Range(0, 100) / 100f * transform.localScale.z - transform.localScale.z / 2;
                Debug.Log(scaleX);
                float x = spawnerPosition.x + scaleX;
                float z = spawnerPosition.z + scaleZ;
                Vector3 position = world.getHeight(new Vector3(x, 0, z));

                int type = Random.Range(0,10);
                if(type>0)
                    ProjectileConcreteFactory.ConstructEnemy(position, world.transform, world, world.turret, 150 + world.waveCounter*25);
                else
                    ProjectileConcreteFactory.ConstructWallbreaker(position, world.transform, world, world.turret, 25 + world.waveCounter*5);

                
                if(counter%(world.waveCounter*10)==0){
                    counter = 0;
                    world.waveCounter++;
                }
            }
        }
    }
}
