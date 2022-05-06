
using UnityEngine;
using System;
public class ExplosionConcreteFactory
{

    public static Explosion ConstructExplosion(Vector3 position, Transform transform, TerrainManager manager)
    {
        GameObject en = new GameObject("Explosion");
        en.transform.localPosition = position;
        en.transform.parent = transform;
        Explosion explosion = en.AddComponent<Explosion>();
        explosion.manager = manager;
        Debug.Log("test" + explosion);

        return explosion;
    }
}