using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Enemy : Entity
{
    bool time = true;
    bool threadStart = true;
    public Entity goal;
    float timer;
    public float hp = 100;

    private Pathfinding.Input input;
    private List<Vector3> path = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        world.enemyList.Add(this);
    }

    void OnTriggerEnter(Collider col)
    {
        hp -= 40;
        if (hp < 0)
        {

            delete();
            world.gold++;
        }
        Projectile projectile = col.gameObject.GetComponent<Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (threadStart && goal != null)
        {
            threadStart = false;
            Thread thread = new Thread(Pathfinding.calculatePath);
            input = new Pathfinding.Input(world.getIndexPosition(transform.position), world.getIndexPosition(goal.transform.position), world, new List<Vector3>());
            thread.Start(input);
        }
        if (input != null && input.finished && time)
        {
            foreach (Vector3 item in input.result)
            {
                path.Add(item);
                time = false;
            }

        }

        if (path.Count > 0 && !time)
        {

            if (Math.Abs(transform.position.x - path[0].x) > 0.1f || Math.Abs(transform.position.z - path[0].z) > 0.1f)
            {
                Vector3 wantToMove = world.getHeight(path[0]);
                Vector3 direction = wantToMove - transform.position;
                float angle = Vector3.Angle(Vector3.up, direction) - 90;

                if (angle < -45f)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, wantToMove.y, transform.position.z), 0.001f);
                else
                    transform.position = Vector3.MoveTowards(transform.position, wantToMove, 0.02f);
            }
            else
            {
                path.RemoveAt(0);

                Debug.DrawRay(world.getHeight(transform.position), Vector3.up, Color.blue, 500);
            }
        }
        if (!time && path.Count == 0)
        {
            world.turret.hp -= 100;
            delete();
        }



    }

    private void delete()
    {
        world.enemyList.Remove(this);
        Destroy(gameObject);
        Destroy(this);
    }
}
