using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    protected List<Projectile> projectileList;
    public virtual void Start() {
        
    }

    public void init(List<Projectile> list){
        projectileList = list;
    }
}
