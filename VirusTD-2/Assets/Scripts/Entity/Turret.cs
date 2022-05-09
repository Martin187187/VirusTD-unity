using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{
    
        protected void delete()
    {
        world.turretList.Remove(this);
        Destroy(gameObject);
        Destroy(this);
    }
}
