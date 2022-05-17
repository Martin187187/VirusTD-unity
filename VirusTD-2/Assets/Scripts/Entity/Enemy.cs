using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public abstract class Enemy : Entity
{
    protected bool time = true;
    protected bool threadStart = true;
    public Entity goal;
    protected float timer;
    public float hp = 100;

    protected Pathfinding.Input input;
    protected List<Vector3> path = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        world.enemyList.Add(this);
    }
    void OnTriggerEnter(Collider col)
    {
        Turret turret = col.gameObject.GetComponent<Turret>();
        hp -= 40;
        if (hp < 0)
        {

            delete();
            world.gold++;
        }
    }

    protected void move(float pathfindingScale)
    {
        if (threadStart && goal != null)
        {
            threadStart = false;
            Thread thread = new Thread(Pathfinding.calculatePath);
            input = new Pathfinding.Input(world.getIndexPosition(transform.position), world.getIndexPosition(goal.transform.position), world, pathfindingScale, new List<Vector3>());
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
                MakeAktion();
            }
            else
            {
                path.RemoveAt(0);

                Debug.DrawRay(world.getHeight(transform.position), Vector3.up, Color.blue, 3);
            }
        }
        if (!time && path.Count == 0)
        {
            world.turret.hp -= 100;
            delete();
        }
    }

    protected abstract void MakeAktion();
    

    protected void delete()
    {
        world.enemyList.Remove(this);
        Destroy(gameObject);
        Destroy(this);
    }
}
