using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainManager : MonoBehaviour
{
    
    List<Projectile> entityList;
    Block[,,] blockArray;
    public Vector3Int numberOfChunks = new Vector3Int(1, 2, 1);
    public int chunkLength = 8;
    public int verteciesInChunk = 3;
    public Material m_material;
    // Start is called before the first frame update



    private Vector3 savedPosition;
    private bool isSaved = false;
    void Start()
    {
        blockArray = new Block[numberOfChunks.x, numberOfChunks.y, numberOfChunks.z];

        GameObject terrain = new GameObject("Terrain");
        terrain.transform.parent = transform;
        for (int x = 0; x < numberOfChunks.x; x++)
        {
            for (int y = 0; y < numberOfChunks.y; y++)
            {
                for (int z = 0; z < numberOfChunks.z; z++)
                {
                    GameObject ob = new GameObject("Block");
                    ob.transform.localPosition = new Vector3(x*chunkLength, y*chunkLength, z*chunkLength);
                    ob.transform.localScale = new Vector3(chunkLength/(verteciesInChunk-1f), chunkLength/(verteciesInChunk-1f), chunkLength/(verteciesInChunk-1f));
                    ob.transform.parent = terrain.transform;
                    Block b = ob.AddComponent<Block>();
                    blockArray[x, y, z] = b;
                    b.index = new Vector3Int(x,y,z);
                    b.gridSize = verteciesInChunk;
                    b.material = m_material;
                }
            }
        }
        entityList = new List<Projectile>();
        GameObject en = new GameObject("Projectile");

        en.transform.localPosition = new Vector3(4, 25, 4);
        en.transform.localScale = new Vector3(1/4f, 1/4f, 1/4f);
        en.transform.parent = transform;
        Projectile p = en.AddComponent<Projectile>();
        p.gridSize = 5;
        p.material = m_material;
        entityList.Add(p);

    }

    // Update is called once per frame
    void Update()
    {

        foreach (Projectile p in entityList)
        {
            //terrain hit
            Vector3Int position = getBlockPosition(p.getPosition());

            //TODO negative values could be possible later...
            //updates if it hit any terrain
            if (isInBounderies(position))
            {

                Block currentBlock = blockArray[position.x, position.y, position.z];
                currentBlock.subtractMesh(p);
                currentBlock.updateMesh();
                currentBlock.gameObject.GetComponent<MeshCollider>().sharedMesh = currentBlock.GetComponent<MeshFilter>().mesh;

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                    Vector3 position = hit.point;

                if (Input.GetKey(KeyCode.LeftShift)) { 
                    //terrain hit

                    if (isInBounderies(getBlockPosition(position)))
                    {

                        if (!isSaved)
                        {
                            Debug.Log("save Position");
                            savedPosition = position;
                            isSaved = true;

                        }
                        else
                        {
                            Debug.Log("dig Hole");
                            flatTerrain(savedPosition, position);
                            isSaved = false;
                        }
                    }


                } else
                {
                    int xa = ((int)(position.x +0.5f) * (verteciesInChunk-1))/(verteciesInChunk-1);
                    int ya = ((int)(position.y +0.5f) * (verteciesInChunk-1))/(verteciesInChunk-1);
                    int za = ((int)(position.z +0.5f) * (verteciesInChunk-1))/(verteciesInChunk-1);

                    Vector3Int roundedPosition = new Vector3Int(xa, ya, za);
                    flatTerrain(roundedPosition - new Vector3Int(2,0,2), roundedPosition + new Vector3Int(2,0,2));

                    GameObject ob = Instantiate(Resources.Load<GameObject>("Tower/Machinegun"), roundedPosition - new Vector3(0.25f,0.25f,0.25f), Quaternion.identity);
                    ob.transform.parent = transform;
                    ob.AddComponent<Animator>();
                    ob.AddComponent<Machinegun>();
                }
            
            
            
            
            
            
            
            }
        }
    }

    public void flatTerrain(Vector3 first, Vector3 second){
        
        Vector3Int savedBlockPosition = getBlockPosition(first);
        Vector3Int blockPosition = getBlockPosition(second);
        int startX = (int)Mathf.Min(savedBlockPosition.x, blockPosition.x);
        int endX = (int)Mathf.Max(savedBlockPosition.x, blockPosition.x);
        //int startY = (int)Mathf.Min(savedBlockPosition.y, blockPosition.y);
        //int endY = (int)Mathf.Max(savedBlockPosition.y, blockPosition.y);
        int startY = 0;
        int endY = savedBlockPosition.y;
        int startZ = (int)Mathf.Min(savedBlockPosition.z, blockPosition.z);
        int endZ = (int)Mathf.Max(savedBlockPosition.z, blockPosition.z);

        float blockStartX = Mathf.Min(first.x, second.x);
        float blockEndX = Mathf.Max(first.x, second.x);
        //float blockStartY = Mathf.Min(first.y, second.y);
        //float blockEndY = Mathf.Max(first.y, second.y);
        float blockStartY = 0;
        float blockEndY = first.y;
        float blockStartZ = Mathf.Min(first.z, second.z);
        float blockEndZ = Mathf.Max(first.z, second.z);

        Debug.Log(blockEndY + "26.3: " + (int)((blockEndY % chunkLength) / 0.5f + 0.5f));
        for (int i = startX; i <= endX; i++)
        {
            for (int k = startY; k <= endY; k++)
            {
                for (int j = startZ; j <= endZ; j++)
                {
                    Block block = blockArray[i, k, j];

                    Vector3 scale = block.getScale();
                    int gridStartX = i == startX ? (int)((blockStartX % chunkLength) / scale.x + 0.5f) : 0;
                    int gridStartY = k == startY ? (int)((blockStartY % chunkLength) / scale.y + 0.5f) : 0;
                    int gridStartZ = j == startZ ? (int)((blockStartZ % chunkLength) / scale.z + 0.5f) : 0;

                    int gridEndX = (i == endX) ? (int)((blockEndX % chunkLength) / scale.x + 0.5f) : verteciesInChunk;
                    int gridEndY = (k == endY) ? (int)((blockEndY % chunkLength) / scale.y + 0.5f) : verteciesInChunk;
                    int gridEndZ = (j == endZ) ? (int)((blockEndZ % chunkLength) / scale.z + 0.5f) : verteciesInChunk;

                    Vector3Int gridStartPosition = new Vector3Int(gridStartX, gridStartY, gridStartZ);
                    Vector3Int gridEndPosition = new Vector3Int(gridEndX, gridEndY, gridEndZ);

                    block.digHole(gridStartPosition, gridEndPosition);
                    block.gameObject.GetComponent<MeshCollider>().sharedMesh = block.GetComponent<MeshFilter>().mesh;
                }
            }
        }
    }

    public Vector3Int getBlockPosition(Vector3 position){
        float part = (float)1/chunkLength;
        int x = (int)(position.x / chunkLength);
        int y = (int)(position.y / chunkLength);
        int z = (int)(position.z / chunkLength);

        return new Vector3Int(x,y,z);
    }
    public bool isInBounderies(Vector3Int position){
        return position.x>=0 && position.x < numberOfChunks.x && position.y>=0 && position.y < numberOfChunks.y && position.z>=0 && position.z < numberOfChunks.z;
    }
}
