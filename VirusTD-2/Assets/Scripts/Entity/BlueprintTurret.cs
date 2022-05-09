using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintTurret : Blueprint
{
    // Start is called before the first frame update
    GameObject sphere;
    void Start()
    {


        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.GetComponent<Renderer>().material = Resources.Load("Materials/Wireframe/Wireframe", typeof(Material)) as Material;
        sphere.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        sphere.transform.SetParent(transform);
        sphere.transform.position = transform.position;
        sphere.transform.localScale *= 40;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            delete();
        }
        else
        {

            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1 << 3))
            {
                Vector3 position = hit.point;
                int scale = (world.gridSize - 1) / world.chunkLength;
                Vector3 scaledPosition = (position * scale);
                scaledPosition = new Vector3((int)scaledPosition.x, scaledPosition.y, (int)scaledPosition.z) / scale;
                Collider[] hitColliders = Physics.OverlapBox(scaledPosition, Vector3.one * 2, Quaternion.identity, 1 << 6);

                Vector3 first = scaledPosition - new Vector3(1, 0, 1) * 1;
                Vector3 second = scaledPosition + new Vector3(1, 0, 1) * 2;
                transform.position = scaledPosition;

                int count = 10 * (world.turretList.Count + 1);
                if (hitColliders.Length == 0 && world.checkHeight(first, second) && world.gold>=count)
                {
                    GetComponent<MeshRenderer>().material.color = Color.green;


                    if (Input.GetMouseButtonDown(0))
                    {
                        delete();
                        Turret turtle = ProjectileConcreteFactory.ConstructTarget(transform.position, world.transform, world);
                        world.turretList.Add(turtle);
                        world.gold -= count;
                    }
                }
                else
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }


}
