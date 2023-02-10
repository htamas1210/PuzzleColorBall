using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    private CameraController cc;
    //public float moveSpeed = 5f;
    public float jumpforce = 5f;
    private float sideMovement = 3f;
    private Vector3 direction;
    private float horizontal, vertical;
    private bool isJumping;
    private bool isOnGround;


    //
    public float holdTime = 0.3f;
    private bool isTapped = false;
    private float timeSinceLastTap = 0f;

    //swipe movement
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private void Awake() {
        cc = FindObjectOfType<CameraController>();
    }

    private void Update(){
        //jumping
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began){
                isTapped = true;
                timeSinceLastTap = Time.time;
            }

            if(touch.phase == TouchPhase.Ended){
                isTapped = false;
                timeSinceLastTap = 0f;
            }

            if(isTapped && rb.transform.position.y <= 0.16f){
                if(Time.time - timeSinceLastTap >= holdTime){
                    Debug.Log("Long tapped");
                    rb.AddForce(new Vector3(0, jumpforce, 0));
                    isTapped = false;
                }
            }
        }

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

    private void goLeft(){
        if(rb.transform.position.x >= 2.5f) return; //ne tudjon kimenni a savbol
        cc.xPostion = -3;
        //rb.transform.position = new Vector3(rb.transform.position.x + sideMovement, rb.transform.position.y, rb.transform.position.z);
        rb.AddForce(new Vector3(300f, 0, 0));
    }

    private void goRight(){
        if(rb.transform.position.x <= -2.5f) return; //ne tudjon kimenni a savbol
        cc.xPostion = 3;
        //rb.transform.position = new Vector3(rb.transform.position.x - sideMovement, rb.transform.position.y, rb.transform.position.z);
        rb.AddForce(new Vector3(-300f, 0, 0));
    }
}
