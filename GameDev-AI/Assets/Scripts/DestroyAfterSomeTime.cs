using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSomeTime : MonoBehaviour {
    [SerializeField]
    private float timeBeforeDestroy = 5f;
    private float currentLifeTime;
    void Start() {
        currentLifeTime = 0;
    }

    // Update is called once per frame
    void Update() {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= timeBeforeDestroy) {
            Destroy(gameObject);
        }
    }
}
