using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;

public class Spawner : MonoBehaviour
{
    public TerrainManager world;
    float timer = 0;
    int counter = 0;
    public bool finished = true;
    public int startWave = 0;
    ConcurrentQueue<Pathfinding.Input> inputList = new ConcurrentQueue<Pathfinding.Input>();
    ConcurrentQueue<Pathfinding.Input> inputList2 = new ConcurrentQueue<Pathfinding.Input>();
    bool print2 = false;
    // Start is called before the first frame update
    void Start()
    {
        world.spawners.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        drawPaths(inputList, Color.red);
        //drawPaths(inputList2, Color.green);

        if (!finished && world.waveCounter > startWave)
            if (world.running || counter > 0)
            {
                timer += 0.01f;
                if (timer > 1)
                {
                    timer = 0;
                    counter++;

                    Vector3 spawnerPosition = transform.position;
                    float scaleX = Random.Range(0, 100) / 100f * transform.localScale.x - transform.localScale.x / 2;
                    float scaleZ = Random.Range(0, 100) / 100f * transform.localScale.z - transform.localScale.z / 2;
                    float x = spawnerPosition.x + scaleX;
                    float z = spawnerPosition.z + scaleZ;
                    Vector3 position = world.getHeight(new Vector3(x, 0, z));

                    int type = Random.Range(0, 10);
                    if (type >= 0)
                        ProjectileConcreteFactory.ConstructEnemy(position, world.transform, world, world.turret, 150 + world.waveCounter * 25);
                    else
                        ProjectileConcreteFactory.ConstructWallbreaker(position, world.transform, world, world.turret, 25 + world.waveCounter * 5);


                    if (counter % (world.waveCounter * 10) == 0)
                    {
                        counter = 0;
                        finished = true;
                    }
                }
            }

    }

    void OnMouseDown()
    {
        Debug.Log("lol");
        showPaths(inputList, 3);
        //showPaths(inputList2, 1);
    }

    public void showPaths(ConcurrentQueue<Pathfinding.Input> queue, float scale)
    {
        print2 = true;
        Vector3 spawnerPosition = transform.position;
        for (float i = 0; i <= 100; i = i + 10)
        {
            float scaleX = i / 100f * transform.localScale.x - transform.localScale.x / 2;
            float scaleZ = i / 100f * transform.localScale.z - transform.localScale.z / 2;
            float x = spawnerPosition.x + scaleX;
            float z = spawnerPosition.z + scaleZ;
            Vector3 position = world.getHeight(new Vector3(x, 0, z));

            Thread thread = new Thread(Pathfinding.calculatePath);
            Pathfinding.Input input = new Pathfinding.Input(world.getIndexPosition(world.getHeight(position)), world.getIndexPosition(world.turret.transform.position), world, scale, new List<Vector3>());
            thread.Start(input);
            queue.Enqueue(input);


        }


    }

    public void drawPaths(ConcurrentQueue<Pathfinding.Input> queue, Color color)
    {
        Pathfinding.Input input;
        queue.TryPeek(out input);
        if (input != null && input.finished)
        {
            queue.TryDequeue(out input);
            foreach (Vector3 vevtor in input.result)
            {
                Vector3 correct = world.getHeight(vevtor);
                Debug.DrawLine(correct, correct + Vector3.up, color, 10);
            }

        }
    }
}
