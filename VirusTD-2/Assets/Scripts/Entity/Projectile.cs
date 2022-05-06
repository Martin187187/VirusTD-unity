using System;
using UnityEngine;

using UnityEditor;
using MarchingCubesProject;
public class Projectile : DynamicMesh
{
    public Vector3 velocity;
    public void Start()
    {

        Tuple<float[], ColorMode[]> result = MeshBuilder.createCubeMesh(gridSize);
        voxels = result.Item1;
        colorList = result.Item2;

        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().materials = materials;
        gameObject.AddComponent<MeshFilter>();
        BoxCollider boxCollider =  gameObject.AddComponent<BoxCollider>();
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        boxCollider.isTrigger = true;
        rigidbody.isKinematic = true;

        updateMesh();

        //AssetDatabase.CreateAsset(GetComponent<MeshFilter>().mesh, "Assets/Resources/test.asset");
        //AssetDatabase.SaveAssets();
    }


    public override void specificUpdate()
    {
    }
    public void Update()
    {
        {

            transform.localPosition = transform.localPosition + velocity * 0.1f;
        }
    }

}
