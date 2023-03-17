using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{   
    private Score score;
    private void Awake() {
        score = FindObjectOfType<Score>();
    }
     private void OnTriggerEnter(Collider other) {
        //Debug.Log("score triggered");
        if(other.gameObject.tag == "Player"){
            score.addScore(1);
        }
     }
}
