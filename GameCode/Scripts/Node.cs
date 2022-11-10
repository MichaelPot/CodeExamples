using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : iHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gCost;
    public int hCost;
    public int gridX, gridY, gridZ;
    public Node parent;

    int heapIndex;

    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY, int gridZ)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return -compare;
    }
}
