using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    //public float moveSpeed = 5f;
    public float jumpforce = 5f;
    private Vector3 direction;
    private float horizontal, vertical, isJumping;
    private bool isOnGround;

    //swipe movement
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        /*horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        isJumping = Input.GetAxis("Jump");*/

        //jumping
        /*if (isJumping > 0 && isOnGround) {
            rb.AddForce(new Vector3(horizontal, jumpforce, vertical));
            isOnGround = false;
        }*/

        /*direction = new Vector3(horizontal, 0,vertical);
        rb.AddForce(direction * moveSpeed * Time.deltaTime);*/



        //new character controller with swipe lane changing
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            startTouchPosition = Input.GetTouch(0).position;
        }
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
            endTouchPosition = Input.GetTouch(0).position;

            if(endTouchPosition.x < startTouchPosition.x){
                //left
                goLeft();

            }else if(endTouchPosition.x > startTouchPosition.x){
                //right
                goRight();
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Ground")) {
            isOnGround = true;
        }
    }

    private void goLeft(){
        rb.transform.position = new Vector3(rb.transform.position.x + 5, rb.transform.position.y, rb.transform.position.z);
    }

    private void goRight(){
        rb.transform.position = new Vector3(rb.transform.position.x - 5, rb.transform.position.y, rb.transform.position.z);
    }
}
