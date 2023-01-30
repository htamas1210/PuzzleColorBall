using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;
    public float jumpforce = 5f;
    private Vector3 direction;
    private float horizontal, vertical, isJumping;
    private bool isOnGround;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        isJumping = Input.GetAxis("Jump");

        if (isJumping > 0 && isOnGround) {
            //rb.AddForce(new Vector3(horizontal, jumpforce, vertical));
            rb.AddForce(new Vector3(vertical, jumpforce, horizontal));
            isOnGround = false;
        }

        direction = new Vector3(horizontal, 0,vertical);

        rb.AddForce(direction * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Ground")) {
            isOnGround = true;
        }
    }
}
