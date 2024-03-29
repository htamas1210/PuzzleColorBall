using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb; //jatekos teste
    private CameraController cc;
    private IsGrounded isGrounded;

    public float jumpforce = 5f; //mekkorat tudjon ugorni
    private float sideMovement = 3f; //oldalra mennyit mozogjon
    private Vector3 direction; //jatkos pozicio
    private bool isJumping; //levegobe van e
    public float holdTime = 0.3f; //meddig kell nyomni egy érintéshez
    private bool isTapped = false; //kattintas erzekeles
    private float timeSinceLastTap = 0f; //mennyi ido telt el a legutolso erintes ota
    private Vector2 startTouchPosition; //erintes kezdo pozicio
    private Vector2 endTouchPosition; //erintes vegpozicio

    [SerializeField] private Button buttonControl;
    [SerializeField] private Button swipeControl;

    public ControlType activeControllType; //ezt kell atallitani hogy swipe-os vagy button-os legyen a mozgas

    public Button leftButton;
    public Button jumpButton;
    public Button rightButton;

    public enum ControlType
    {
        Swipe,
        Button
    }

    private void Awake() {
        cc = FindObjectOfType<CameraController>(); //kamera vezerlo referencia
        isGrounded = FindObjectOfType<IsGrounded>();
        
        if(activeControllType == ControlType.Swipe){
            SwipeControl();
        }
        if(activeControllType == ControlType.Button){
            ButtonControl();
        }
    }

    private void setControllType(ControlType controlltype){
        activeControllType = controlltype;
    }

    public void ButtonControl(){
        setControllType(ControlType.Button);
        swipeControl.image.color = Color.white;
        buttonControl.image.color = Color.red;
    }

    public void SwipeControl(){
        setControllType(ControlType.Swipe);
        swipeControl.image.color = Color.red;
        buttonControl.image.color = Color.white;
    }

    private void Update(){
        
        if(activeControllType == ControlType.Swipe){  
            leftButton.gameObject.SetActive(false);        
            jumpButton.gameObject.SetActive(false);        
            rightButton.gameObject.SetActive(false);

            //jumping
            if(Input.touchCount > 0){
                Touch touch = Input.GetTouch(0); //elso erintes lekerese

                if(touch.phase == TouchPhase.Began){ //ha az erintes elkezdotott
                    isTapped = true;
                    timeSinceLastTap = Time.time;
                }

                if(touch.phase == TouchPhase.Ended){ //ha az erintes befejezodott
                    isTapped = false;
                    timeSinceLastTap = 0f;
                }

                if(isTapped && isGrounded.isGrounded){
                    if(Time.time - timeSinceLastTap >= holdTime){ //ha nyomva tartotta a beallitott ideig
                        Debug.Log("Long tapped");
                        jump();
                        isTapped = false;
                    }
                }
            }

            //new character controller with swipe lane changing
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){ //elso erintes elkezdodott
                startTouchPosition = Input.GetTouch(0).position; 
            }
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){ //elso erintes befejezodott
                endTouchPosition = Input.GetTouch(0).position;

                if(endTouchPosition.x < startTouchPosition.x){ //balra huzott
                    //left
                    goLeft();

                }else if(endTouchPosition.x > startTouchPosition.x){ //jobbra huzott
                    //right
                    goRight();
                }
            }
        }else if(activeControllType == ControlType.Button){
            //jumpforce = 100;
            /*leftButton.onClick.AddListener(goRight);
            jumpButton.onClick.AddListener(jump);
            rightButton.onClick.AddListener(goLeft);*/
        }
    }

    public void goLeft(){ //helyezze at a jatekos objektumot a balra levo savba
        Debug.Log("kattintva bal");
        if(rb.transform.position.x <= -2.5f) return; //ne tudjon kimenni a savbol
        //cc.xPostion = -3; //kamera xPozicioja
        //rb.transform.position = new Vector3(rb.transform.position.x - sideMovement, rb.transform.position.y, rb.transform.position.z);

        if(rb.transform.position.x >= 2.5f){
            rb.transform.position = new Vector3(0, rb.transform.position.y, rb.transform.position.z);
        }else if(rb.transform.position.x >= -2.5f){
            rb.transform.position = new Vector3(-3f, rb.transform.position.y, rb.transform.position.z);
        }
    }

    public void goRight(){ //helyezze at a jatekos objektumot a jobbra levo savba
        Debug.Log("kattintva jobb");
        if(rb.transform.position.x >= 2.5f) return; //ne tudjon kimenni a savbol
        //cc.xPostion = 3; //kamera xPozicioja
        //rb.transform.position = new Vector3(rb.transform.position.x + sideMovement, rb.transform.position.y, rb.transform.position.z);

        if(rb.transform.position.x <= -2.5f){
            rb.transform.position = new Vector3(0, rb.transform.position.y, rb.transform.position.z);
        }else if(rb.transform.position.x >= -2.5f){
            rb.transform.position = new Vector3(3f, rb.transform.position.y, rb.transform.position.z);
        }
    }

    public void jump(){
        if(isGrounded.isGrounded){
            rb.AddForce(new Vector3(0, jumpforce, 0)); //ugras
            isGrounded.isGrounded = false;
        }
  
        Debug.Log("jumped");
    }


}