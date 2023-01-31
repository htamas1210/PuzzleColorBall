using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private GameObject[] ground;
    private float groundMoveSpeed = 10f;


    private void Awake() {
        //getting all of the ground objects by the tag
        ground = GameObject.FindGameObjectsWithTag("Ground");
        if(ground.Length == 0){
            Debug.Log("Nem talalt ground objectet");
        }else{
            Debug.Log("ground length: " + ground.Length);
        }
    }

    private void Update() {
        for (int i = 0; i < ground.Length; i++)
        {
            ground[i].transform.position = ground[i].transform.position + new Vector3(0,0, -groundMoveSpeed * Time.deltaTime);
        }
    }

    private void CheckGroundToDestroy(){
        //z = -80 -nal lehet torolni
        if(ground[0].transform.position.z == 0){
            
        }
    }

}
