using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int gridX;
    public int gridY;

    public bool isWall;
    public Vector3 position;

    public Node parent;

    public int gCost; // The code of moving to the next square
    public int hCost; // The distance to the goel from this node

    public int FCost { get { return gCost + hCost; } } //  gCost and hCost added to each other

    public Node(bool IsWall, Vector3 Pos, int GridX, int GridY) {
        isWall = IsWall;
        position = Pos;
        gridX = GridX;
        gridY = GridY;
    }
}
