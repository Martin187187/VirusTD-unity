using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;

public class Pathfinding
{

    private static int scale = 4;
    public static void calculatePath(object obj)
    {
        Input input = obj as Input;

        List<Vector3Int> result = shortestPath(input.start, input.goal, input.world);

        Debug.Log("finished" + result.Count);
        List<Vector3> rawPath = new List<Vector3>();
        foreach (Vector3 item in result)
        {
            rawPath.Add(new Vector3(item.x, 0, item.z));
        }

        List<Vector3> solution = Curver.MakeSmoothCurve(rawPath, 3.0f);
        foreach (Vector3 item in solution)
        {
            input.result.Add(item);
        }
        input.finished = true;
    }


    private static List<Vector3Int> shortestPath(Vector3Int start, Vector3Int goal, TerrainManager world)
    {

        SortedList<int, NodeResult> needToVisit = new SortedList<int, NodeResult>(new DuplicateKeyComparer<int>());

        List<NodeResult> visted = new List<NodeResult>();
        needToVisit.Add(0, new NodeResult(start));
        int counter = 0;
        while (needToVisit.Count > 0)
        {
            counter++;
            if (counter > 100000)
            {
                return new List<Vector3Int>();
            }

            NodeResult current = needToVisit.Values[0];
            int currentValue = needToVisit.Keys[0];
            if (current.position.x == goal.x && current.position.z == goal.z)
            {
                return current.getPath();
            }
            needToVisit.RemoveAt(0);
            if (!visted.Contains(current))
            {

                visted.Add(current);
                List<Vector3Int> newNodes = current.getNodesToVisit();
                foreach (Vector3Int position in newNodes)
                {
                    if (world.isInWorld(new Vector3Int(position.x * 2, position.y * 2, position.z * 2)) &&
                    world.isInWorld(new Vector3Int(position.x * 2 + 2, position.y * 2, position.z * 2 - 2)) &&
                    world.isInWorld(new Vector3Int(position.x * 2 + 2, position.y * 2, position.z * 2 + 2)) &&
                    world.isInWorld(new Vector3Int(position.x * 2 - 2, position.y * 2, position.z * 2 + 2)) &&
                    world.isInWorld(new Vector3Int(position.x * 2 - 2, position.y * 2, position.z * 2 - 2))&&
                    world.isInWorld(new Vector3Int(position.x * 2 + 2, position.y * 2, position.z * 2 )) &&
                    world.isInWorld(new Vector3Int(position.x * 2 - 2, position.y * 2, position.z * 2 )) &&
                    world.isInWorld(new Vector3Int(position.x * 2, position.y * 2, position.z * 2 + 2)) &&
                    world.isInWorld(new Vector3Int(position.x * 2, position.y * 2, position.z * 2 - 2)))
                    {
                        int height = world.calculateHeight(position.x * 2, position.z * 2);
                        int value = (int)Math.Pow(Math.Abs(current.position.y - height) * scale, 3) + 1;

                        int height1 = world.calculateHeight(position.x * 2 + 2, position.z * 2 - 2);
                        int value1 = (int)Math.Pow(Math.Abs(current.position.y - height1) * scale, 1) + 1;
                        int height2 = world.calculateHeight(position.x * 2 + 2, position.z * 2 + 2);
                        int value2 = (int)Math.Pow(Math.Abs(current.position.y - height2) * scale, 1) + 1;
                        int height3 = world.calculateHeight(position.x * 2 - 2, position.z * 2 + 2);
                        int value3 = (int)Math.Pow(Math.Abs(current.position.y - height3) * scale, 1) + 1;
                        int height4 = world.calculateHeight(position.x * 2 - 2, position.z * 2 - 2);
                        int value4 = (int)Math.Pow(Math.Abs(current.position.y - height4) * scale, 1) + 1;
                        
                        int height5 = world.calculateHeight(position.x * 2 + 2, position.z * 2 - 2);
                        int value5 = (int)Math.Pow(Math.Abs(current.position.y - height5) * scale, 1) + 1;
                        int height6 = world.calculateHeight(position.x * 2 + 2, position.z * 2 + 2);
                        int value6 = (int)Math.Pow(Math.Abs(current.position.y - height6) * scale, 1) + 1;
                        int height7 = world.calculateHeight(position.x * 2 - 2, position.z * 2 + 2);
                        int value7 = (int)Math.Pow(Math.Abs(current.position.y - height7) * scale, 1) + 1;
                        int height8 = world.calculateHeight(position.x * 2 - 2, position.z * 2 - 2);
                        int value8 = (int)Math.Pow(Math.Abs(current.position.y - height8) * scale, 1) + 1;
                        NodeResult newNodeResult = new NodeResult(new Vector3Int(position.x, height, position.z), current);
                        needToVisit.Add(currentValue + (value + value1 + value2 + value3 + value4+ value5 + value6 + value7 + value8) / 9, newNodeResult);
                    }
                }
            }
        }
        return new List<Vector3Int>();
    }

    public class Input
    {
        public Vector3Int start, goal;
        public TerrainManager world;
        public List<Vector3> result;
        public Boolean finished;
        public Input(Vector3Int start, Vector3Int goal, TerrainManager world, List<Vector3> result)
        {
            this.start = start;
            this.goal = goal;
            this.world = world;
            this.result = result;
            this.finished = false;
        }
    }
    public class NodeResult : IEquatable<NodeResult>
    {

        public Vector3Int position;
        public NodeResult parent;
        public NodeResult(Vector3Int position)
        {
            this.position = position;
            this.parent = null;
        }
        public NodeResult(Vector3Int position, NodeResult parent)
        {
            this.position = position;
            this.parent = parent;
        }

        public List<Vector3Int> getNodesToVisit()
        {
            List<Vector3Int> nodes = new List<Vector3Int>();
            nodes.Add(new Vector3Int(position.x, position.y, position.z - 1));
            nodes.Add(new Vector3Int(position.x + 1, position.y, position.z));
            nodes.Add(new Vector3Int(position.x, position.y, position.z + 1));
            nodes.Add(new Vector3Int(position.x - 1, position.y, position.z));
            return nodes;
        }


        public List<Vector3Int> getPath()
        {
            List<Vector3Int> path = new List<Vector3Int>();
            NodeResult current = this;
            while (current != null)
            {
                path.Add(current.position);
                current = current.parent;
            }
            path.Reverse();
            return path;
        }

        public override int GetHashCode()
        {
            return 0;
        }
        public override bool Equals(object obj)
        {
            NodeResult node = obj as NodeResult;
            if (node == null)
            {
                return false;
            }
            return Equals(node);
        }

        public bool Equals(NodeResult node)
        {
            return node.position.x == position.x && node.position.z == position.z;

        }
    }
    public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1; // Handle equality as being greater. Note: this will break Remove(key) or
            else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                return result;
        }

        #endregion
    }

}
