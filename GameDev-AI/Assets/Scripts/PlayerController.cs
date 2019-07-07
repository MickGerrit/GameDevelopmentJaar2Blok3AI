using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 4f;
    public float velocitySmoothness = 3f;

    private Vector2 velocity;
    private Vector2 rawVelocityInput;
    private float scale;
    private Rigidbody rb;
    [SerializeField]
    private LayerMask spiderWebLayerMask;
    [SerializeField]
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start() {
        scale = transform.localScale.x;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        rawVelocityInput = Vector2.up * Input.GetAxisRaw("Vertical") + Vector2.right * Input.GetAxisRaw("Horizontal");
    }


    private void FixedUpdate() {
        velocity = Vector2.Lerp(velocity, rawVelocityInput * speed * SpiderWebSpeedMultiplier(), Time.deltaTime * velocitySmoothness);
        rb.MovePosition(transform.position + new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime);
    }

    private void LateUpdate() {
        FlipGameObject();
    }

    private void FlipGameObject() {
        if (sprite == null) {
            return;
        }
        if (velocity.x > 0) {
            sprite.flipX = true;
        } else if (velocity.x < 0) {
            sprite.flipX = false;
        }
    }

    private float SpiderWebSpeedMultiplier() {
        float multiplier = 1f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, 5f, spiderWebLayerMask)) {
            multiplier = 0.5f;
        }
        return multiplier;
    }
}
