using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    string GetName();

    void SetPosition(Vector3 position);
    void SetScale(Vector3 scale);
    void setTransform(Transform transform);
}
