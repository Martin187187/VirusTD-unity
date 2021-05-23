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
        for (int x = 0; x < numberOfChunks.x; x++)
        {
            for (int y = 0; y < numberOfChunks.y; y++)
            {
                for (int z = 0; z < numberOfChunks.z; z++)
                {
                    blockArray[x, y, z] = new Block(new Vector3(x,y,z), new Vector3(chunkLength, chunkLength, chunkLength), verteciesInChunk, transform, m_material);
                }
            }
        }
        entityList = new List<Projectile>();
        entityList.Add(new Projectile(new Vector3(4, 25, 4), new Vector3(1, 1, 1), 5, transform, m_material));
        
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach(Projectile p in entityList)
        {
            //terrain hit
            Vector3Int position = getBlockPosition(p.getPosition());

            //TODO negative values could be possible later...
            //updates if it hit any terrain
            if(isInBounderies(position)){
                
                Block currentBlock = blockArray[position.x, position.y, position.z];
                currentBlock.subtractMesh(p);
                currentBlock.updateMesh();
                currentBlock.GetGameObject().GetComponent<MeshFilter>().mesh = currentBlock.GetMesh();
                currentBlock.GetGameObject().GetComponent<MeshCollider>().sharedMesh = currentBlock.GetMesh();

            }
        }
        
        if(Input.GetMouseButtonDown(0)&&Input.GetKey(KeyCode.LeftShift)){
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;

            if(Physics.Raycast(castPoint, out hit, Mathf.Infinity)){

                //terrain hit
                Vector3Int blockPosition = getBlockPosition(hit.point);
                
                Vector3 position = hit.point;
                Debug.Log("position: " + position.ToString());
                if(isInBounderies(blockPosition)){

                    if(!isSaved){
                        Debug.Log("save Position");
                        savedPosition = position;
                        isSaved = true;
                        
                    } else {
                        Debug.Log("dig Hole");

                        Vector3Int savedBlockPosition = getBlockPosition(savedPosition);

                        int startX = (int)Mathf.Min(savedBlockPosition.x, blockPosition.x);
                        int endX = (int)Mathf.Max(savedBlockPosition.x, blockPosition.x);
                        //int startY = (int)Mathf.Min(savedBlockPosition.y, blockPosition.y);
                        //int endY = (int)Mathf.Max(savedBlockPosition.y, blockPosition.y);
                        int startY = 0;
                        int endY = savedBlockPosition.y;
                        int startZ = (int)Mathf.Min(savedBlockPosition.z, blockPosition.z);
                        int endZ = (int)Mathf.Max(savedBlockPosition.z, blockPosition.z);

                        float blockStartX = Mathf.Min(savedPosition.x, position.x);
                        float blockEndX = Mathf.Max(savedPosition.x, position.x);
                        //float blockStartY = Mathf.Min(savedPosition.y, position.y);
                        //float blockEndY = Mathf.Max(savedPosition.y, position.y);
                        float blockStartY = 0;
                        float blockEndY = savedPosition.y;
                        float blockStartZ = Mathf.Min(savedPosition.z, position.z);
                        float blockEndZ = Mathf.Max(savedPosition.z, position.z);

                        Debug.Log(blockEndY+"26.3: " + (int)((blockEndY % chunkLength)/0.5f+0.5f));
                        for(int i = startX; i <= endX; i++){
                            for(int k = startY; k <= endY; k++){
                                for(int j = startZ; j <= endZ; j++){
                                    Block block = blockArray[i, k, j];

                                    Vector3 scale = block.getScale();
                                    int gridStartX = i == startX ? (int)((blockStartX % chunkLength)/scale.x+0.5f) : 0;
                                    int gridStartY = k == startY ? (int)((blockStartY % chunkLength)/scale.y+0.5f) : 0;
                                    int gridStartZ = j == startZ ? (int)((blockStartZ % chunkLength)/scale.z+0.5f) : 0;

                                    int gridEndX = (i == endX) ? (int)((blockEndX % chunkLength)/scale.x+0.5f) : verteciesInChunk;
                                    int gridEndY = (k == endY) ? (int)((blockEndY % chunkLength)/scale.y+0.5f) : verteciesInChunk;
                                    int gridEndZ = (j == endZ) ? (int)((blockEndZ % chunkLength)/scale.z+0.5f) : verteciesInChunk;

                                    Vector3Int gridStartPosition = new Vector3Int(gridStartX, gridStartY, gridStartZ);
                                    Vector3Int gridEndPosition = new Vector3Int(gridEndX, gridEndY, gridEndZ);

                                    block.digHole(gridStartPosition, gridEndPosition);
                                    block.GetGameObject().GetComponent<MeshCollider>().sharedMesh = block.GetMesh();
                                }
                            }
                        }
                        isSaved = false;
                    }
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
