using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunConcreteFactory : TowerAbstractFactory<Machinegun>
{
    
    public override Machinegun ConstructEntity(Vector3 position, Vector3 scale, Transform transform, List<Projectile> projectileList){
        return null;
    }
}
