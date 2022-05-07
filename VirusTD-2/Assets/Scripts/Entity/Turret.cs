using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{
    public Entity target;
    // Start is called before the first frame update
    void Start()
    {
        world.turretList.Add(this);
    }
    float timer = 0;
    bool reload = false;
    // Update is called once per frame
    void Update()
    {
        if (target == null)
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

                    if (Physics.Raycast(castPoint, out hit, Mathf.Infinity) && hit.collider.gameObject.layer != 3)
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
            float angle = Vector3.Angle(position2, transform.position);
            transform.GetChild(0).GetChild(0).Rotate(new Vector3(0, position2.y > transform.position.y ? angle : -angle, 0));

            if (timer >= 0.5 && !reload)
            {

                Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z);

                Vector3 direction = target.transform.position - projectilePosition;
                direction.Normalize();

                Ray castPoint = new Ray(projectilePosition, direction);
                RaycastHit hit;

                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {

                    Vector3 hitPosition = hit.point;
                    if (hit.collider.gameObject.layer != 3)
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
