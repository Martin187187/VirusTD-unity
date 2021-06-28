using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileConcreteFactory 
{
    
    public Projectile ConstructEntity(Vector3 position, Vector3 scale, Transform transform, int gridSize, Vector3 velocity){
        GameObject en = new GameObject("Projectile");

        en.transform.localPosition = position;
        en.transform.localScale = scale;
        en.transform.parent = transform;
        Projectile p = en.AddComponent<Projectile>();
        p.gridSize = 5;
        p.materials = new Material[1];
        p.materials[0] = Resources.Load("Materials/Material", typeof(Material)) as Material;
        p.velocity = velocity;
        return p;
    }

}
