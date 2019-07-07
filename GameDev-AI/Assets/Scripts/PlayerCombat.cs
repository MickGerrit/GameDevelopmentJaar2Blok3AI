using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    private Vector3 direction;
    public GameObject boomerangGameObject;
    public GameObject instantiatedBoomerang;
    public Transform throwPoint;

    public bool hasBoomerang;

    private bool canThrowBoomerang;

    public float timeBetweenWhips = 1f;
    private float whipTimer;
    public LayerMask whipLayerMask;
    public float whipLength = 5f;
    public int whipDamage = 1;
    // Start is called before the first frame update
    void Start() {
        hasBoomerang = true;
        canThrowBoomerang = true;
    }

    // Update is called once per frame
    void Update() {
        Timers();
        if (Input.GetAxisRaw("Fire2") > 0 && hasBoomerang && canThrowBoomerang) {
            ThrowBoomerang();
        }
        //if (Input.GetAxisRaw("Fire2") < 0 && whipTimer >= timeBetweenWhips) {
        //    Debug.Log("Whip");
        //    Whip();
        //}
        if (Input.GetAxisRaw("Fire2") == 0) {
            canThrowBoomerang = true;
        }

        direction = new Vector3(Input.GetAxisRaw("HorizontalRight"), 0, Input.GetAxisRaw("VerticalRight")).normalized;
        direction *= 5;
    }

    private void ThrowBoomerang() {
        instantiatedBoomerang = Instantiate(boomerangGameObject, throwPoint.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        
        direction += Vector3.up * throwPoint.position.y;
        instantiatedBoomerang.GetComponent<Boomerang>().direction = direction;
        hasBoomerang = false;
        canThrowBoomerang = false;
    }

    private void Whip() {
        RaycastHit hit;
        bool isHit = Physics.Linecast(transform.position, direction*whipLength + transform.position, out hit, whipLayerMask);
        if (isHit) {
            Debug.Log("Hit");
            if (hit.transform.tag == "Enemy") {
                if (hit.transform.GetComponent<Health>() != null) {
                    hit.transform.GetComponent<Health>().Damage(whipDamage);
                }
            }
        }
        whipTimer = 0;
    }

    private void Timers() {
        if (whipTimer < timeBetweenWhips) {
            whipTimer += Time.deltaTime;
        }
    }

    private void OnDrawGizmos() {
        Debug.DrawLine(transform.position, transform.position + direction*whipLength, Color.blue);
    }
}
