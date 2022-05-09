using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainManager : MonoBehaviour
{

    public List<Projectile> entityList;
    Block[,,] blockArray;
    public Vector3Int numberOfChunks = new Vector3Int(1, 2, 1);
    public int chunkLength = 8;
    public int gridSize = 3;
    // Start is called before the first frame update

    public TerrainCharacteristic characteristic;

    private Vector3 savedPosition;
    private bool isSaved = false;
    public Base turret;
    public Enemy enemy;
    public Blueprint blueprint;

    public List<Enemy> enemyList = new List<Enemy>();
    public List<Turret> turretList = new List<Turret>();
    public int gold = 100;

    public bool running = false;
    public int waveCounter = 1;
    void Start()
    {
        Application.targetFrameRate = 144;
        entityList = new List<Projectile>();
        MeshBuilder.init(characteristic);

        blockArray = new Block[numberOfChunks.x, numberOfChunks.y, numberOfChunks.z];

        GameObject terrain = new GameObject("Terrain");
        terrain.transform.parent = transform;
        for (int x = 0; x < numberOfChunks.x; x++)
        {
            for (int y = 0; y < numberOfChunks.y; y++)
            {
                for (int z = 0; z < numberOfChunks.z; z++)
                {
                    blockArray[x, y, z] = Singleton.blockFactory.ConstructEntity(new Vector3Int(x, y, z), terrain.transform, chunkLength, gridSize);
                }
            }
        }
        entityList.Add(ProjectileConcreteFactory.ConstructEntity(new Vector3(4, 40, 4), new Vector3(1 / 4f, 1 / 4f, 1 / 4f), transform, 5, new Vector3(0, -0.1f, 0)));

    }
    // Update is called once per frame
    void Update()
    {
        if (turret == null)
            turret = ProjectileConcreteFactory.ConstructBase(getHeight(new Vector3(64, 0, 64)), transform, this);
        for (int i = 0; i < entityList.Count; i++)
        {
            Projectile p = entityList[i];
            //terrain hit
            Vector3Int position = getBlockPosition(p.getPosition());

            //TODO negative values could be possible later...
            //updates if it hit any terrain
            if (p.DidTravelLongEnough() && isInBounderies(position))
            {

                Block currentBlock = blockArray[position.x, position.y, position.z];

                if (currentBlock.WouldIntersect(p.transform.localPosition))
                {
                    currentBlock.updateMesh();
                    entityList.Remove(p);
                    Destroy(p.gameObject);
                }

            }
            else if (p.transform.localPosition.magnitude > 100)
            {
                entityList.Remove(p);
                Destroy(p.gameObject);
            }
        }

        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            Vector3 position = hit.point;


            if (Input.GetMouseButtonDown(0))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
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
                            Vector3 distance = savedPosition - position;
                            float cost = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(Mathf.Max(savedPosition.y, position.y), 2) + Mathf.Pow(distance.z, 2));
                            if (gold >= cost)
                            {
                                flatTerrain(savedPosition, position);
                                Debug.Log("lool: " + checkHeight(savedPosition, position));
                                gold -= (int)cost;
                            }
                            isSaved = false;
                        }
                    }


                }
                else
                {
  
                    //enemy = ProjectileConcreteFactory.ConstructEnemy(position, transform, this, turret);
                    /*
                    Vector3 movedPosition = position + castPoint.direction * chunkLength / gridSize / 2;
                    ExplosionConcreteFactory.ConstructExplosion(movedPosition, transform, this);
                    List<Block> blockList = getAllBlocksFromPosition(movedPosition);
                    foreach (Block block in blockList)
                    {
                        block.intersect(movedPosition);
                        block.updateMesh();
                    }
                    */
                    /*
                    int count = 10 * (turretList.Count + 1);
                    if (gold >= count)
                    {
                        int scale = (gridSize - 1) / chunkLength;
                        Vector3 scaledPosition = (position * scale);
                        scaledPosition = new Vector3((int)scaledPosition.x, scaledPosition.y, (int)scaledPosition.z) / scale;
                        Collider[] hitColliders = Physics.OverlapBox(scaledPosition, Vector3.one * 2, Quaternion.identity, 1 << 6);

                        Vector3 first = scaledPosition - new Vector3(1, 0, 1) * 1;
                        Vector3 second = scaledPosition + new Vector3(1, 0, 1) * 2;
                        if (hitColliders.Length == 0 && checkHeight(first, second))
                        {


                            Turret turtle = ProjectileConcreteFactory.ConstructTarget(scaledPosition, transform, this);
                            turretList.Add(turtle);
                            gold -= count;
                        }
                    }
                    */
                }







            }
            if (blueprint == null)
            {
                if (Input.GetKeyDown(KeyCode.T))
                    blueprint = ProjectileConcreteFactory.ConstructBlueprint(position, transform, this);
                else if (Input.GetKeyDown(KeyCode.L))
                    blueprint = ProjectileConcreteFactory.ConstructLaserLine(position, transform, this);
            }
        }


    }
    public int calculateHeight(int x, int z)
    {


        return blockArray[x / (gridSize - 1), 0, z / (gridSize - 1)].heightMap[x % (gridSize - 1), z % (gridSize - 1)];
    }
    public Vector3 getHeight(Vector3 position)
    {
        Ray castPoint = new Ray(new Vector3(position.x, 100, position.z), new Vector3(0, -1, 0));
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1 << 3))
        {
            return hit.point;
        }
        return new Vector3(position.x, 0, position.z);
    }
    public void reloadTerrain()
    {
        for (int x = 0; x < numberOfChunks.x; x++)
        {
            for (int y = 0; y < numberOfChunks.y; y++)
            {
                for (int z = 0; z < numberOfChunks.z; z++)
                {
                    blockArray[x, y, z].rebuild();
                    blockArray[x, y, z].gameObject.GetComponent<MeshCollider>().sharedMesh = blockArray[x, y, z].GetComponent<MeshFilter>().mesh;
                }
            }
        }
    }

    public void flatTerrain(Vector3 first, Vector3 second)
    {

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

                    int gridEndX = (i == endX) ? (int)((blockEndX % chunkLength) / scale.x + 0.5f) : gridSize;
                    int gridEndY = (k == endY) ? (int)((blockEndY % chunkLength) / scale.y + 0.5f) : gridSize;
                    int gridEndZ = (j == endZ) ? (int)((blockEndZ % chunkLength) / scale.z + 0.5f) : gridSize;

                    Vector3Int gridStartPosition = new Vector3Int(gridStartX, gridStartY, gridStartZ);
                    Vector3Int gridEndPosition = new Vector3Int(gridEndX, gridEndY, gridEndZ);

                    block.digHole(gridStartPosition, gridEndPosition);
                    block.gameObject.GetComponent<MeshCollider>().sharedMesh = block.GetComponent<MeshFilter>().mesh;
                }
            }
        }
    }

    public void explodeTerrain(Vector3 first, Vector3 second)
    {

        Vector3Int savedBlockPosition = getBlockPosition(first);
        Vector3Int blockPosition = getBlockPosition(second);
        int startX = (int)Mathf.Min(savedBlockPosition.x, blockPosition.x);
        int endX = (int)Mathf.Max(savedBlockPosition.x, blockPosition.x);
        //int startY = (int)Mathf.Min(savedBlockPosition.y, blockPosition.y);
        //int endY = (int)Mathf.Max(savedBlockPosition.y, blockPosition.y);
        int startY = 0;
        int endY = 0;
        int startZ = (int)Mathf.Min(savedBlockPosition.z, blockPosition.z);
        int endZ = (int)Mathf.Max(savedBlockPosition.z, blockPosition.z);

        float blockStartX = Mathf.Min(first.x, second.x);
        float blockEndX = Mathf.Max(first.x, second.x);
        //float blockStartY = Mathf.Min(first.y, second.y);
        //float blockEndY = Mathf.Max(first.y, second.y);
        float blockStartY = Mathf.Min(first.y, second.y);
        float blockEndY = gridSize;
        float blockStartZ = Mathf.Min(first.z, second.z);
        float blockEndZ = Mathf.Max(first.z, second.z);

        Debug.Log(blockEndY + "26.3: " + (int)((blockEndY % chunkLength) / 0.5f + 0.5f));
        for (int i = startX; i <= endX; i++)
        {
            for (int k = startY; k <= endY; k++)
            {
                for (int j = startZ; j <= endZ; j++)
                {
                    Debug.Log("i: " + i + ", j: " + j + "k: " + k);
                    Block block = blockArray[i, k, j];

                    Vector3 scale = block.getScale();
                    int gridStartX = i == startX ? (int)((blockStartX % chunkLength) / scale.x + 0.5f) : 0;
                    int gridStartY = k == startY ? (int)((blockStartY % chunkLength) / scale.y + 0.5f) : 0;
                    int gridStartZ = j == startZ ? (int)((blockStartZ % chunkLength) / scale.z + 0.5f) : 0;

                    int gridEndX = (i == endX) ? (int)((blockEndX % chunkLength) / scale.x + 0.5f) : gridSize;
                    int gridEndY = gridSize;
                    int gridEndZ = (j == endZ) ? (int)((blockEndZ % chunkLength) / scale.z + 0.5f) : gridSize;

                    Vector3Int gridStartPosition = new Vector3Int(gridStartX, gridStartY, gridStartZ);
                    Vector3Int gridEndPosition = new Vector3Int(gridEndX, gridEndY, gridEndZ);

                    block.fill(gridStartPosition, gridEndPosition);
                    block.gameObject.GetComponent<MeshCollider>().sharedMesh = block.GetComponent<MeshFilter>().mesh;
                }
            }
        }
    }

    public bool checkHeight(Vector3 first, Vector3 second)
    {
        Vector3Int savedBlockPosition = getBlockPosition(first);
        Vector3Int blockPosition = getBlockPosition(second);
        int startX = (int)Mathf.Min(savedBlockPosition.x, blockPosition.x);
        int endX = (int)Mathf.Max(savedBlockPosition.x, blockPosition.x);
        //int startY = (int)Mathf.Min(savedBlockPosition.y, blockPosition.y);
        //int endY = (int)Mathf.Max(savedBlockPosition.y, blockPosition.y);
        int startZ = (int)Mathf.Min(savedBlockPosition.z, blockPosition.z);
        int endZ = (int)Mathf.Max(savedBlockPosition.z, blockPosition.z);

        float blockStartX = Mathf.Min(first.x, second.x);
        float blockEndX = Mathf.Max(first.x, second.x);
        //float blockStartY = Mathf.Min(first.y, second.y);
        //float blockEndY = Mathf.Max(first.y, second.y);
        float blockStartY = Mathf.Min(first.y, second.y);
        float blockEndY = gridSize;
        float blockStartZ = Mathf.Min(first.z, second.z);
        float blockEndZ = Mathf.Max(first.z, second.z);

        int height = 0;
        bool setHeight = true;
        for (int i = startX; i <= endX; i++)
        {
            for (int j = startZ; j <= endZ; j++)
            {
                Block block = blockArray[i, 0, j];

                Vector3 scale = block.getScale();
                int gridStartX = i == startX ? (int)((blockStartX % chunkLength) / scale.x + 0.5f) : 0;
                int gridStartY = (int)((blockStartY % chunkLength) / scale.y + 0.5f);
                int gridStartZ = j == startZ ? (int)((blockStartZ % chunkLength) / scale.z + 0.5f) : 0;

                int gridEndX = (i == endX) ? (int)((blockEndX % chunkLength) / scale.x + 0.5f) : gridSize;
                int gridEndY = gridSize;
                int gridEndZ = (j == endZ) ? (int)((blockEndZ % chunkLength) / scale.z + 0.5f) : gridSize;

                Vector3Int gridStartPosition = new Vector3Int(gridStartX, gridStartY, gridStartZ);
                Vector3Int gridEndPosition = new Vector3Int(gridEndX, gridStartY, gridEndZ);

                for (int x = gridStartPosition.x; x < gridEndPosition.x; x++)
                {
                    for (int z = gridStartPosition.z; z < gridEndPosition.z; z++)
                    {
                        int current = block.heightMap[x, z];
                        if (setHeight)
                        {
                            height = current;
                            setHeight = false;
                        }
                        else if (height != current)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }
    public Vector3Int getIndexPosition(Vector3 position)
    {

        int x = Mathf.RoundToInt(position.x - position.x % ((float)1 / (gridSize - 1)));
        int y = Mathf.RoundToInt(position.y - position.y % ((float)1 / (gridSize - 1)));
        int z = Mathf.RoundToInt(position.z - position.z % ((float)1 / (gridSize - 1)));

        return new Vector3Int(x, y, z);
    }

    public bool isInWorld(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x - position.x % ((float)1 / (gridSize - 1)));
        int y = Mathf.RoundToInt(position.y - position.y % ((float)1 / (gridSize - 1)));
        int z = Mathf.RoundToInt(position.z - position.z % ((float)1 / (gridSize - 1)));

        return !(x < 0 || y < 0 || z < 0 || x >= (gridSize - 1) * numberOfChunks.x || y >= (gridSize - 1) * numberOfChunks.y || z >= (gridSize - 1) * numberOfChunks.z);
    }
    public Vector3Int getBlockPosition(Vector3 position)
    {
        int x = (int)(position.x / chunkLength);
        int y = (int)(position.y / chunkLength);
        int z = (int)(position.z / chunkLength);

        return new Vector3Int(x, y, z);
    }

    public List<Block> getAllIntersectingBlockPosition(Vector3 position)
    {
        List<Vector3Int> blockPositionList = new List<Vector3Int>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    int x = (int)((position.x - i) / chunkLength);
                    int y = (int)((position.y - j) / chunkLength);
                    int z = (int)((position.z - k) / chunkLength);

                    Vector3Int b = new Vector3Int(x, y, z);
                    if (!blockPositionList.Contains(b))
                        blockPositionList.Add(b);
                }
            }
        }
        List<Block> blockList = new List<Block>();
        foreach (Vector3Int vec in blockPositionList)
            blockList.Add(getBlockFromIndex(vec));
        return blockList;
    }

    public bool isInBounderies(Vector3Int position)
    {
        return position.x >= 0 && position.x < numberOfChunks.x && position.y >= 0 && position.y < numberOfChunks.y && position.z >= 0 && position.z < numberOfChunks.z;
    }

    public Block getBlockFromPosition(Vector3 position)
    {
        Vector3Int b = getBlockPosition(position);
        return blockArray[b.x, b.y, b.z];
    }

    public Block getBlockFromIndex(Vector3Int b)
    {
        return blockArray[b.x, b.y, b.z];
    }

    public float getScale()
    {
        return 1f * chunkLength / (gridSize - 1);
    }

    public List<Block> getAllBlocksFromPosition(Vector3 position)
    {

        Vector3Int blockPosition = getBlockPosition(position);
        List<Block> blockList = new List<Block>();

        float x = (position / chunkLength).x % 1 * (gridSize - 1);
        float y = (position / chunkLength).y % 1 * (gridSize - 1);
        float z = (position / chunkLength).z % 1 * (gridSize - 1);
        bool isxBorder = x < 1;
        bool isyBorder = y < 1;
        bool iszBorder = z < 1;
        bool isxborderR = x > (gridSize - 2);
        bool isyborderR = y > (gridSize - 2);
        bool iszborderR = z > (gridSize - 2);

        Debug.Log("xborder: " + (position / chunkLength).x);
        blockList.Add(getBlockFromIndex(blockPosition));
        if (isxBorder)
        {
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), blockPosition.y, blockPosition.z)));
        }
        if (isyBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, Mathf.Max(0, blockPosition.y - 1), blockPosition.z)));
        if (iszBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, blockPosition.y, Mathf.Max(0, blockPosition.z - 1))));

        if (isxBorder && isyBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), Mathf.Max(0, blockPosition.y - 1), blockPosition.z)));
        if (iszBorder && isyBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, Mathf.Max(0, blockPosition.y - 1), Mathf.Max(0, blockPosition.z - 1))));
        if (isxborderR && isyBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), Mathf.Max(0, blockPosition.y - 1), blockPosition.z)));
        if (iszborderR && isyBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, Mathf.Max(0, blockPosition.y - 1), Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));

        if (isxBorder && isyBorder && iszBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), Mathf.Max(0, blockPosition.y - 1), Mathf.Max(0, blockPosition.z - 1))));
        if (isxborderR && isyBorder && iszBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), Mathf.Max(0, blockPosition.y - 1), Mathf.Max(0, blockPosition.z - 1))));
        if (isxBorder && isyBorder && iszborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), Mathf.Max(0, blockPosition.y - 1), Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));
        if (isxborderR && isyBorder && iszborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), Mathf.Max(0, blockPosition.y - 1), Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));

        if (isxborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), blockPosition.y, blockPosition.z)));
        if (isyborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), blockPosition.z)));
        if (iszborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, blockPosition.y, Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));
        if (isxBorder && isyborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), blockPosition.z)));
        if (iszBorder && isyborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), Mathf.Max(0, blockPosition.z - 1))));
        if (isxborderR && isyborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), blockPosition.z)));
        if (iszborderR && isyborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(blockPosition.x, Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));

        if (isxBorder && isyborderR && iszBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), Mathf.Max(0, blockPosition.z - 1))));
        if (isxborderR && isyborderR && iszBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), Mathf.Max(0, blockPosition.z - 1))));
        if (isxBorder && isyborderR && iszborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));
        if (isxborderR && isyborderR && iszborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), Mathf.Min(numberOfChunks.y - 1, blockPosition.y + 1), Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));

        if (isxBorder && iszBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), blockPosition.y, Mathf.Max(0, blockPosition.z - 1))));
        if (isxborderR && iszBorder)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), blockPosition.y, Mathf.Max(0, blockPosition.z - 1))));
        if (isxBorder && iszborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Max(0, blockPosition.x - 1), blockPosition.y, Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));
        if (isxborderR && iszborderR)
            blockList.Add(getBlockFromIndex(new Vector3Int(Mathf.Min(numberOfChunks.x - 1, blockPosition.x + 1), blockPosition.y, Mathf.Min(numberOfChunks.z - 1, blockPosition.z + 1))));
        /*
        */
        return blockList;
    }
}
