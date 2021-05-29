using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun : Tower
{
    public Transform target;
    private float timeCount = 0f;
    private int counter = 0;
    private float rotationSpeed = 0.01f;
    public override void Start() {
        gameObject.AddComponent<BoxCollider>();
    }

    public void Update() {
        
        if(target!=null){

            if(aimAt(target)){
                if(counter >=19){
                    shoot();
                    counter = 0;
                }
                counter = counter +1;
            }
        } else {
            timeCount = 0f;
        }
    }

    public void shoot(){
        GameObject projectile = new GameObject("Projectile");
        
        Quaternion direction = transform.GetChild(0).transform.localRotation * transform.GetChild(0).GetChild(0).transform.localRotation;
        projectile.transform.localPosition = transform.localPosition + direction * new Vector3(0, 1.834f, 2.338f);
        projectile.transform.localScale = new Vector3(1/32f, 1/32f, 1/32f);

        Projectile p = projectile.AddComponent<Projectile>();
        p.gridSize = 4;
        p.velocity = direction * new Vector3(0, 0, 1);
        p.material = Resources.Load("Material/Material", typeof(Material)) as Material;
        projectileList.Add(p);
    }

    public bool aimAt(Transform target){
            
            Transform yTarget = transform.GetChild(0);
            Vector3 y = new Vector3(target.localPosition.x, transform.localPosition.y, target.localPosition.z);
            Vector3 directionY = (y-transform.localPosition).normalized;
            Quaternion rotY = Quaternion.LookRotation(directionY);
            yTarget.rotation = Quaternion.Slerp(yTarget.rotation, rotY, timeCount * rotationSpeed);

            Transform xTarget = yTarget.GetChild(0);
            Vector3 x = new Vector3(target.localPosition.x, target.localPosition.y, target.localPosition.z);
            Vector3 directionX = (x-transform.localPosition).normalized;
            Quaternion rotX = Quaternion.LookRotation(directionX);
            xTarget.rotation = Quaternion.Slerp(xTarget.rotation, rotX, timeCount * rotationSpeed);
            
            if(Mathf.Abs(Quaternion.Angle(rotX, xTarget.rotation))<2&&Mathf.Abs(Quaternion.Angle(rotY, yTarget.rotation))<2)
                return true;
            
 
            

            timeCount = timeCount + Time.deltaTime;
            return false;

    }

}
