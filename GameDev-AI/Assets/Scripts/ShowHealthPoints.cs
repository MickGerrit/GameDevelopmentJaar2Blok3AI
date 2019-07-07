using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHealthPoints : MonoBehaviour
{
    private Text textComponent;
    private Health health;
    // Start is called before the first frame update
    void Start() {
        textComponent = GetComponent<Text>();
        health = GetComponentInParent<Health>();
    }

    // Update is called once per frame
    void Update() {
        textComponent.text = (Mathf.RoundToInt(health.currentHealthPoints / health.maximumHealthPoints * 100f)).ToString() + "%";
    }
}
