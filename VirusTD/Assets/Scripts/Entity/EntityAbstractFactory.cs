using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAbstractFactory
{
    protected abstract IEntity MakeEntity();

    public IEntity GetEntity(){
        return this.MakeEntity();
    }
}
