using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunConcreteFactory : TowerAbstractFactory<Machinegun>
{
    
    public override Machinegun ConstructEntity(Vector3 position, Vector3 scale, Transform transform, List<Projectile> projectileList){
        GameObject ob = GameObject.Instantiate(Resources.Load<GameObject>("Tower/Machinegun"), position - new Vector3(0.25f,0.25f,0.25f), Quaternion.identity);
        ob.transform.parent = transform;
        ob.AddComponent<Animator>();
        Machinegun gun = ob.AddComponent<Machinegun>();
        gun.projectileList = projectileList;
        return gun;
    }
}
