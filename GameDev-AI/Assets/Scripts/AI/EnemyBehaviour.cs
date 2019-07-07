using Panda;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    private MovementAlongFinalPath movement;
    private Pathfinding pathFinding;
    private Health health;
    private float currentMoveSpeed;
    private float standardMoveSpeed;
    private Transform player;
    private PlayerCombat playerCombat;
    [SerializeField]
    private Transform secondaryTarget;
    private Vector3 fleePositionTarget;
    [SerializeField]
    private LayerMask playerLayerMask;
    [SerializeField]
    private float rangeBeforeAttack = 2f;
    [Task]
    private bool playerInAttackRange;
    [SerializeField]
    private float rangeBeforeSprint = 5f;
    [Task]
    private bool playerInSprintRange;
    [SerializeField]
    private float sprintRangeMargin = 2f;

    [SerializeField]
    private float distanceBeforeBoomerangDodge = 5f;

    [Task]
    private bool playerHasBoomerang { get { return playerCombat.hasBoomerang;  } set { playerHasBoomerang = value;} }

    [Task]
    private bool canDodge;
    private float hp { get { return health.currentHealthPoints; } set { hp = value; } }

    private BigEnemyBehaviour bigEnemyBehaviour;

    private int healthPickUpIndex;

    [SerializeField]
    private LayerMask spiderWebLayerMask;
    
    [SerializeField]
    private ParticleSystem spiderGooParticles;
    [SerializeField]
    private AudioSource spiderGooSFX;

    private void Start() {
        bigEnemyBehaviour = GameObject.FindObjectOfType<BigEnemyBehaviour>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canDodge = true;
        movement = GetComponent<MovementAlongFinalPath>();
        pathFinding = GetComponentInChildren<Pathfinding>();
        pathFinding.targetPosition = player;
        playerCombat = player.GetComponent<PlayerCombat>();
        health = GetComponent<Health>();

        currentMoveSpeed = movement.moveSpeed;
        standardMoveSpeed = currentMoveSpeed;

        playerInAttackRange = false;
        playerInSprintRange = false;
        secondaryTarget.parent = null;
    }

    private void Update() {

        if (playerHasBoomerang) {
            canDodge = true;
        }

        float distanceFromPlayer = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));
        if (distanceFromPlayer > rangeBeforeAttack) {
            playerInAttackRange = false;
        } else {
            playerInAttackRange = true;
        }

        if (distanceFromPlayer > rangeBeforeSprint) {
            playerInSprintRange = false;
        } else if (distanceFromPlayer < (rangeBeforeSprint - sprintRangeMargin)) {
            playerInSprintRange = true;
        }

        SpeedUpFromSpiderWeb();
    }

    [Task]
    private void DontMove() {
        SetMovementSpeed(0);
        Task.current.Succeed();
    }

    [Task]
    private void MoveToPlayer() {
        SetPathFindingTarget(player);
        SetMovementSpeed(currentMoveSpeed);
        Task.current.Succeed();
    }

    [Task]
    private void Sprint() {
        SetPathFindingTarget(player);
        SetMovementSpeed(standardMoveSpeed * 2f);
        Task.current.Succeed();
    }

    [Task]
    private bool IsBoomerangNear() {
        if (playerCombat.instantiatedBoomerang == null) {
            return false;
        }
        if (Vector3.Distance(playerCombat.instantiatedBoomerang.transform.position, transform.position) < distanceBeforeBoomerangDodge) {
            return true;
        } else return false;
    }


    [Task]
    private void DodgeDiagonal() {
        int randomNegativeOrPositive = Random.Range(0, 2) == 0 ? -1 : 1;
        secondaryTarget.position = transform.position + randomNegativeOrPositive * transform.right * 3;
        SetPathFindingTarget(secondaryTarget);
        SetCurrentRotation(randomNegativeOrPositive * -90);
        SetMovementSpeed(standardMoveSpeed * 2);
        Task.current.Succeed();
    }

    [Task]
    private bool HasDodged() {
        canDodge = false;
        return true;
    }

    [Task]
    private bool IsLowHealth() {
        return hp <= 3;
    }

    [Task]
    private void SetCurrentRotation(float yRotation) {
        movement.currentYRotationOffset = yRotation;
        Task.current.Succeed();
    }

    [Task]
    private bool AttackPlayer() {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, 2f, playerLayerMask);
        spiderGooParticles.Play();
        spiderGooSFX.Play();

        if (!isHit) {
            return isHit;
        }
        if (hit.transform.GetComponent<Health>() != null) {
            hit.transform.GetComponent<Health>().Damage(0.5f);
        }
        return isHit;
    }

    [Task]
    private void SetTargetToRandomHealthPoint() {
        healthPickUpIndex = -1;
        for (int i = 0; i < bigEnemyBehaviour.healthPickUpValue.Length; i++) {
            if (bigEnemyBehaviour.healthPickUpValue[i] >= 1) {
                healthPickUpIndex = i;
                SetPathFindingTarget(bigEnemyBehaviour.healthSpots[healthPickUpIndex]);
                bigEnemyBehaviour.enemyHealRequests[healthPickUpIndex] = true;
                Task.current.Succeed();
                return;
            }
        }
        if (healthPickUpIndex == -1) {
            Task.current.Fail();
        }
    }

    [Task]
     private void HealFromPickUp() {
        if (Vector3.Distance(transform.position, bigEnemyBehaviour.healthSpots[healthPickUpIndex].position) < 1) {
            if (health.currentHealthPoints != health.maximumHealthPoints) {
                health.currentHealthPoints += health.maximumHealthPoints * Time.deltaTime * bigEnemyBehaviour.rechargeSpeed;
                bigEnemyBehaviour.healthPickUpValue[healthPickUpIndex] -= Time.deltaTime * bigEnemyBehaviour.rechargeSpeed;
            }
            if (health.currentHealthPoints >= health.maximumHealthPoints) {
                health.currentHealthPoints = health.maximumHealthPoints;
                bigEnemyBehaviour.enemyHealRequests[healthPickUpIndex] = false;
                Task.current.Succeed();
            }
        } 
    }

    private void SpeedUpFromSpiderWeb() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, 5f, spiderWebLayerMask)) {
            currentMoveSpeed = standardMoveSpeed * 2;
        } else {
            currentMoveSpeed = standardMoveSpeed;
        }
    }

    private void SetPathFindingTarget(Transform target) {
        if (pathFinding.targetPosition != target) {
            pathFinding.targetPosition = target;
        }
    }

    private void SetMovementSpeed(float speed) {
        if (movement.moveSpeed != speed) {
            movement.moveSpeed = speed;
        }
    }
}
