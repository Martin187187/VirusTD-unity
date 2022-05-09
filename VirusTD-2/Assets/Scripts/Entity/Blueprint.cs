using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Blueprint : Entity
{
    // Start is called before the first frame update

    protected void delete()
    {
        Destroy(gameObject);
    }
}
