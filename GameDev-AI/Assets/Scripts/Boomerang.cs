using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour {
    public Vector3 direction;
    public Vector3 destination;
    public Transform player;
    private PlayerCombat playerCombat;
    public bool reachedGoal;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start() {
        destination = transform.position + direction;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCombat = player.GetComponent<PlayerCombat>();
    }

    private void FixedUpdate() {
        if (Vector3.Distance(transform.position, destination) > 0.1f && !reachedGoal) {
            transform.position += direction.normalized * Time.deltaTime * speed; 
        } else if (Vector3.Distance(transform.position, destination) <= 0.1f) {
            reachedGoal = true;
        }

        if (reachedGoal) {
            transform.position += (player.position - transform.position).normalized * Time.deltaTime * speed;
        }

        if (Vector3.Distance(transform.position, player.position) <= 0.1f && reachedGoal) {
            playerCombat.hasBoomerang = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Player") {
            reachedGoal = true;
        }
        if (other.tag == "Enemy") {
            if (other.GetComponent<Health>() != null) {
                other.GetComponent<Health>().Damage(1);
            }
        }
    }

}
