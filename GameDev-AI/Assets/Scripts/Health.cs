using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float maximumHealthPoints = 3;
    public float currentHealthPoints;
    // Start is called before the first frame update
    void Start() {
        currentHealthPoints = maximumHealthPoints;
    }
    private void Update() {
        if (currentHealthPoints <= 0) {
            Destroy(gameObject);
        }
    }
    public void Damage(float amount) {
        currentHealthPoints -= amount;
    }
}
