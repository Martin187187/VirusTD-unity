using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallbreaker : Enemy
{

    // Update is called once per frame
    void Update()
    {
        move(1);
    }

    protected override void MakeAktion()
    {
        Vector3 wantToMove = world.getHeight(path[0]);
        Vector3 direction = wantToMove - transform.position;
        float angle = Vector3.Angle(Vector3.up, direction) - 90;

        if (angle < -45f)
        {
            Vector3 first = transform.position - new Vector3(1, 0, 1) * 3;
            Vector3 second = transform.position + new Vector3(1, 0, 1) * 3;

            if (world.isInBounderies(world.getBlockPosition(first)) && world.isInBounderies(world.getBlockPosition(second)))
            {
                world.explodeTerrain(first, second);
                delete();
            }
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, wantToMove, 0.02f);
    }
}
