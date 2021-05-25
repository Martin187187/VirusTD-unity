using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun : Tower
{
    public Transform target;
    float counter = 0;
    public override void Start() {
    }

    public void Update() {
        
        transform.GetChild(0).Rotate(new Vector3(0,0.2f,0));
        if(counter==19){
            shoot();
            counter = 0;
        }
        counter++;
    }

    public void shoot(){
        GameObject projectile = new GameObject("Projectile");
        
        Quaternion direction = transform.GetChild(0).transform.localRotation;
        projectile.transform.localPosition = transform.localPosition + direction * new Vector3(0, 1.834f, 2.338f);
        projectile.transform.localScale = new Vector3(1/32f, 1/32f, 1/32f);

        Projectile p = projectile.AddComponent<Projectile>();
        p.gridSize = 4;
        p.velocity = direction * new Vector3(0, 0, 1);
        p.material = Resources.Load("Material/Material", typeof(Material)) as Material;
        projectileList.Add(p);
    }

    public void aimAt(Transform target){
            Transform yTarget = transform.GetChild(0);
            Transform xTarget = yTarget.GetChild(0);

            yTarget.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            xTarget.LookAt(new Vector3(target.position.x, target.position.y, target.position.z));
    }
}
