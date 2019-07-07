using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAlongFinalPath : MonoBehaviour {

    public Grid grid;
    public List<Node> finalPath { get { if (grid != null) { return grid.finalPath; } else return null; } }
    
    private float currentSqrMagnitude;

    private Pathfinding pathfindingReference;
    private Vector3 myManhattenPosition;

    [SerializeField] private float sqrMagnitudeBeforeContinuing = 0.2f;
    public float moveSpeed = 2f;
    [SerializeField] private float rotationalLerpToTarget = 2f;

    public Vector3 targetPosition;

    private int wayPointIndex;

    public Vector3 velocityDirection;
    public float currentYRotationOffset;
    private Quaternion lookAtPosition;

    private void Awake() {
        pathfindingReference = GetComponentInChildren<Pathfinding>();
        grid = GetComponentInChildren<Grid>();
    }

    private void Update() {

        if (grid == null) {
            return;
        }
        if (finalPath == null) {
            return;
        }

        if (wayPointIndex >= finalPath.Count) {
            return;
        }

        if (pathfindingReference.isCalculatingPath) {
            wayPointIndex = 0;
            targetPosition = finalPath[wayPointIndex].position;
        }

        WayPointSystem();

        if (finalPath.Count > 0) {
            Movement();
            pathfindingReference.canCalculate = true;
        } else {
            pathfindingReference.canCalculate = false;
        }
    }

    private void Movement() {
        velocityDirection = (new Vector3(targetPosition.x, transform.position.y, targetPosition.z) - transform.position).normalized;
        transform.position += velocityDirection * Time.deltaTime * moveSpeed;
        lookAtPosition = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocityDirection) * Quaternion.Euler(0, currentYRotationOffset, 0), Time.deltaTime * rotationalLerpToTarget);
        transform.rotation = lookAtPosition;
    }

    private void WayPointSystem() {
            myManhattenPosition = new Vector3(transform.position.x, 0, transform.position.z);
            Debug.Log(wayPointIndex);
            currentSqrMagnitude = (myManhattenPosition - finalPath[wayPointIndex].position).sqrMagnitude;

            if (currentSqrMagnitude < sqrMagnitudeBeforeContinuing) {
                if (wayPointIndex >= finalPath.Count) {
                    wayPointIndex += 1;
                    targetPosition = finalPath[wayPointIndex].position;
                }
            }
    }
}
