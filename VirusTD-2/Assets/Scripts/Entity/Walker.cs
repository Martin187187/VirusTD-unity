using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{




    // Update is called once per frame
    void Update()
    {
        move(3);
    }

    protected override void MakeAktion()
    {
        Vector3 wantToMove = world.getHeight(path[0]);
        Vector3 direction = wantToMove - transform.position;
        float angle = Vector3.Angle(Vector3.up, direction) - 90;

        if (angle < -45f)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, wantToMove.y, transform.position.z), 0.001f);
        else
            transform.position = Vector3.MoveTowards(transform.position, wantToMove, 0.02f);
    }

}
