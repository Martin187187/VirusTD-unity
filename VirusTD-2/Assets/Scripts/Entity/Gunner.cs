using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Turret
{
    public Entity target;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 6;
        gameObject.AddComponent<BoxCollider>();
    }
    float timer = 0;
    bool reload = false;
    // Update is called once per frame
    void Update()
    {

        Vector3 first = transform.position - new Vector3(1, 0, 1) * 1;
        Vector3 second = transform.position + new Vector3(1, 0, 1) * 2;
        if (!world.checkHeight(first, second))
        {
            delete();
        }
        else if (target == null)
                {
                    Entity nearestEnemy = null;
                    float distance = 20;
                    foreach (Enemy enemy in world.enemyList)
                    {
                        float value = Vector3.Distance(enemy.transform.position, transform.position);
                        if (value < distance)
                        {
                            Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z);

                            Vector3 direction = enemy.transform.position - projectilePosition;
                            direction.Normalize();

                            Ray castPoint = new Ray(projectilePosition, direction);
                            RaycastHit hit;

                            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity) && hit.collider.gameObject.layer != 3 && hit.collider.gameObject.layer != 6)
                            {
                                nearestEnemy = enemy;
                                distance = value;
                            }
                        }
                    }
                    target = nearestEnemy;
                }
                else
                {
                    timer += 0.01f;
                    Vector3 position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    transform.GetChild(0).LookAt(position, Vector3.up);
                    transform.GetChild(0).Rotate(0, 90, 0);
                    Vector3 position2 = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
                    transform.GetChild(0).GetChild(0).localRotation = new Quaternion(-90, 0, 0, 90);
                    float angle = Vector3.Angle(position2, transform.position)*Mathf.PI*2;
                    transform.GetChild(0).GetChild(0).Rotate(new Vector3(0, position2.y > transform.position.y ? angle : -angle, 0));
                    if (timer >= 0.5 && !reload)
                    {

                        Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);

                        Vector3 direction = target.transform.position - projectilePosition;
                        direction.Normalize();

                        Ray castPoint = new Ray(projectilePosition, direction);
                        RaycastHit hit;

                        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                        {

                            if (hit.collider.gameObject.layer != 3 && hit.collider.gameObject.layer != 6 && Vector3.Distance(target.transform.position, transform.position)<20)
                            {
                                Projectile newProjectile = ProjectileConcreteFactory.ConstructEntity(projectilePosition, Vector3.one * 0.25f, world.transform, 3, direction * 4.0f);
                                world.entityList.Add(newProjectile);
                                timer = 0;
                                reload = false;
                            }
                            else
                            {
                                target = null;
                            }
                        }
                    }

                }


    }

}
