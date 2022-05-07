using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileConcreteFactory 
{
    
    public static Projectile ConstructEntity(Vector3 position, Vector3 scale, Transform transform, int gridSize, Vector3 velocity){
        GameObject en = new GameObject("Projectile");

        en.transform.localPosition = position;
        en.transform.localScale = scale;
        en.transform.parent = transform;
        Projectile p = en.AddComponent<Projectile>();
        p.gridSize = gridSize;
        p.materials = new Material[1];
        p.materials[0] = Resources.Load("Materials/Material", typeof(Material)) as Material;
        p.velocity = velocity;
        return p;
    }

    public static Enemy ConstructEnemy(Vector3 position, Transform transform, TerrainManager world, Entity target, float hp){
        GameObject en = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        en.name = "Enemy";

        en.transform.localPosition = position;
        en.transform.parent = transform;
        en.GetComponent<MeshRenderer>().material.color = Color.red;
        Enemy enemy = en.AddComponent<Enemy>();
        enemy.hp = hp;
        enemy.world = world;
        enemy.goal = target;
        return enemy;
    }

    public static Turret ConstructTarget(Vector3 position, Transform transform, TerrainManager world){
        GameObject en = GameObject.Instantiate(Resources.Load<GameObject>("Assets/gun"), position , Quaternion.identity);
        en.name = "Turret";
        en.transform.parent = transform;

        Turret target = en.AddComponent<Turret>();
        target.world = world;
        return target;
    }

    public static Base ConstructBase(Vector3 position, Transform transform, TerrainManager world){
        GameObject en = GameObject.Instantiate(Resources.Load<GameObject>("Assets/gun"), position , Quaternion.identity);
        en.name = "Base";
        en.transform.parent = transform;
        en.GetComponent<MeshRenderer>().material.color = Color.blue  ;
        en.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue  ;
        en.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue  ;

        Base basen = en.AddComponent<Base>();
        basen.world = world;
        return basen;
    }

}
