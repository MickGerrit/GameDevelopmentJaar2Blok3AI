using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    private Vector3 positionOffset;
    // Start is called before the first frame update
    void Start() {
        positionOffset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, target.position + positionOffset, Time.deltaTime * 5f);
    }
}
