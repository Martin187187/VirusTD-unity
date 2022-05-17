using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintLaserLine : Blueprint
{
    void Start()
    {
        Destroy(gameObject.GetComponent<BoxCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1 << 3))
            {

                float distance = Vector3.Distance(hit.point, transform.position);
                int cost = (int)(distance * (world.turretList.Count + 1));

                Vector3 height = world.getHeight(hit.point);
                if (height.y >= hit.point.y)
                {
                    if (world.gold >= cost)
                    {

                        GameObject second = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        second.name = "first";
                        second.transform.localPosition = hit.point;
                        second.transform.parent = transform;
                        second.GetComponent<MeshRenderer>().material.color = Color.green;
                        Destroy(second.GetComponent<BoxCollider>());
                        LaserLine laser = gameObject.AddComponent<LaserLine>();
                        laser.world = world;
                        laser.otherPart = second;
                        world.gold -= cost;
                        world.turretList.Add(laser);
                        Destroy(this);
                    }
                    else
                    {
                        delete();
                    }
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            delete();
        }
    }

}
