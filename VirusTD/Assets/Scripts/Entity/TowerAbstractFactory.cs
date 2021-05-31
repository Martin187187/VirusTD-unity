using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAbstractFactory<T> where T : Tower
{

    public abstract T ConstructEntity(Vector3 position, Vector3 scale, Transform transform, List<Projectile> projectileList);

}
