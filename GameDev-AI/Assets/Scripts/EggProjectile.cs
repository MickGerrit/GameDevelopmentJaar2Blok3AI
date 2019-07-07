using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggProjectile : MonoBehaviour {
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private float lifeTime = 1f;
    private float timer;
    [SerializeField]
    private GameObject spiderWeb;
    [SerializeField]
    private float spiderWebHeightOffset = -1f;

    private void Start() {
        timer = lifeTime;
    }
    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            CrackEgg();
        }
    }
    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void CrackEgg() {
        Instantiate(spiderWeb, transform.position + transform.up * spiderWebHeightOffset, transform.rotation);
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision) {
        Transform shotParent = collision.transform.root;
        if (shotParent.tag == "Player") {
            if (shotParent.GetComponent<Health>() != null) {
                shotParent.GetComponent<Health>().Damage(1);
            }
        }
        CrackEgg();
    }
}
