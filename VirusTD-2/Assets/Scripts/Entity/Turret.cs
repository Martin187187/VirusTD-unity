using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{
    public float damage = 20;
    protected void delete()
    {
        world.turretList.Remove(this);
        Destroy(gameObject);
        Destroy(this);
    }
}
