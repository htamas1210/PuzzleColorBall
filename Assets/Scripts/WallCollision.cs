using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WallCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            Debug.Log("neki ment a falnak");
            
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
