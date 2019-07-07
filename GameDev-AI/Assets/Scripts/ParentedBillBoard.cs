using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentedBillBoard : MonoBehaviour {
    private Transform cam;
    // Start is called before the first frame update
    void Start() {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.parent.LookAt(cam.position, Vector3.up);
    }
}
