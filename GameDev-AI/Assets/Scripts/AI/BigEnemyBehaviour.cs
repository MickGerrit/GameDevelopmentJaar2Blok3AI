using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class BigEnemyBehaviour : MonoBehaviour {
    [SerializeField]
    private Transform player;
    private Vector3 currentPosition;
    [SerializeField]
    private Vector3 targetPosition;
    private float targetRotation;

    [SerializeField]
    private LayerMask layerMask;
    private Vector3 positionDifference;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject bullet;

    public Transform[] healthSpots;
    public bool[] enemyHealRequests;
    public float[] healthPickUpValue;
    private Health healthReference;
    public float rechargeSpeed = 0.2f;

    [SerializeField]
    [Task]
    private bool gotHurt;

    // Start is called before the first frame update
    void Start() {
        targetPosition = transform.position;
        healthPickUpValue = new float[healthSpots.Length];
        enemyHealRequests = new bool[healthSpots.Length];
        healthReference = GetComponent<Health>();
        gotHurt = false;
        for (int i = 0; i < healthPickUpValue.Length; i++) {
            if (healthPickUpValue[i] <= 1f) {
                healthPickUpValue[i] = 1;
            }
        }
    }

    private bool IsPlayerInCertainDistance(float distance) {
        bool inDistance = false;
        if (Vector3.Distance(player.position, transform.position) <= distance) {
            inDistance = true;
        }
        return inDistance;
    }

    [Task]
    private bool SetTargetPositionTowardsPlayer () {
        currentPosition = transform.position;
        positionDifference = Vector3.zero;
        if (!IsPlayerInCertainDistance(10f)) {
            positionDifference -= transform.right * Random.Range(-5f, 5f);
            positionDifference += transform.forward * Random.Range(0.1f, 3.0f);
        }
        if (IsPlayerInCertainDistance(10f)) {
            positionDifference -= transform.forward * Random.Range(0.1f, 3.0f);
        }
        targetPosition = positionDifference + transform.position;
        return true;
    }

    [Task]
    private bool SetTargetToSafePosition() {
        targetPosition = -transform.forward * 5f;
        return true;
    }

    [Task]
    private bool CalculateTargetRotation() {
        float rotationDifference = Vector3.SignedAngle(transform.forward, player.position - transform.position, Vector3.up);
        if (rotationDifference > 90) {
            targetRotation = transform.eulerAngles.y + rotationDifference;
        } else if (rotationDifference < 90) {
            targetRotation = transform.eulerAngles.y + rotationDifference;
        } else {
            targetRotation = transform.eulerAngles.y;
        }

        return true;
    }

    [Task]
    private void Move() {
        if (transform.position != targetPosition) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f);
        } else {
            Task.current.Succeed();
        }
    }

    [Task]
    private void RotateTowardsPlayer() {
        Vector3 yTargetRotation = new Vector3(0, targetRotation, 0);

        if (transform.rotation != Quaternion.Euler(yTargetRotation)) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(yTargetRotation), Time.deltaTime * 100f);
        } else {
            Task.current.Succeed();
        }
    }

    [Task]
    private void GenerateHealthPickUps() {
        for (int i = 0; i < healthPickUpValue.Length; i++) {
            if (healthPickUpValue[i] <= 1f) {
                healthPickUpValue[i] += Time.deltaTime * rechargeSpeed;
            } else {
                healthPickUpValue[i] = 1;
            }
        }
        Task.current.Succeed();
    }

    [Task]
    private void Shoot() {
        Instantiate(bullet, firePoint.position, transform.rotation);
        Task.current.Succeed();
    }
    [Task]
    private void RotateAfterHit() {
        transform.Rotate(new Vector3(0, Time.deltaTime * 180, 0));
    }

    [Task]
    private bool NotHurtAnymore() {
        return !(gotHurt = false);
    }

    [Task]
    private bool NeedToRecharge() {
        bool recharge = false;
        for (int i = 0; i < healthPickUpValue.Length; i++) {
            if (healthPickUpValue[i] < 1f) {
                recharge = true;
            } 
        }
        return recharge;
    }

    [Task]
    private bool EnemyRequestsToHeal() {
        bool requesting = false;
        for (int i = 0; i < enemyHealRequests.Length; i++) {
            if (enemyHealRequests[i]) {
                requesting = true;
            }
        }
        return requesting;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Boomerang") {
            if (healthReference.currentHealthPoints >= 0) {
                gotHurt = true;
                Debug.Log("Hurt");
            }
        }
    }
}
