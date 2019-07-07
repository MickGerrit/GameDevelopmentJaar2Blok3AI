using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour{

    protected Grid grid;
    public Transform startPosition;
    public Transform targetPosition;

    public bool isCalculatingPath;
    public bool canCalculate;

    protected float nextPathUpdateTime = 0.0f;
    public float pathUpdatePeriod = 0.2f;

    private void Awake() {
        canCalculate = true;
        isCalculatingPath = false;
        grid = GetComponent<Grid>();
        nextPathUpdateTime = Random.Range(0.0f, pathUpdatePeriod);
    }

    private void Update() {
        if (Time.time > nextPathUpdateTime && canCalculate) {
            isCalculatingPath = true;
            nextPathUpdateTime += pathUpdatePeriod;
            FindPath(startPosition.position, targetPosition.position);
        } else {
            isCalculatingPath = false;
        }
            
    }

    private void FindPath(Vector3 StartPosition, Vector3 TargetPosition) {
        Node startNode = grid.NodeFromWorldPosition(StartPosition);
        Node targetNode = grid.NodeFromWorldPosition(TargetPosition);

        List<Node> openList = new List<Node>();
        //using a hashset for the closed 
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);
        
        while(openList.Count > 0) {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost) {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode) {
                GetFinalPath(startNode, targetNode);
                break;
            }

            foreach(Node neighbourNode in grid.GetNeighBourNodes(currentNode)) {
                if (!neighbourNode.isWall || closedList.Contains(neighbourNode)) {
                    continue;
                }

                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighbourNode);

                if (moveCost < neighbourNode.gCost || !openList.Contains(neighbourNode)) {
                    neighbourNode.gCost = moveCost;
                    neighbourNode.hCost = GetManhattenDistance(neighbourNode, targetNode);
                    neighbourNode.parent = currentNode;

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }

        }
    }

    private void GetFinalPath(Node StartingNode, Node EndNode) {
        List<Node> finalPath = new List<Node>();
        Node currentNode = EndNode;

        while(currentNode != StartingNode) {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();

        grid.finalPath = finalPath;
    }

    private int GetManhattenDistance(Node NodeA, Node NodeB) {
        int ix = Mathf.Abs(NodeA.gridX - NodeB.gridX);
        int iy = Mathf.Abs(NodeA.gridY - NodeB.gridY);

        return ix + iy;
    }
}
