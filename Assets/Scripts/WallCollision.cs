using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WallCollision : MonoBehaviour
{
    jatekmanager jatekmanager;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){

            Debug.Log("neki ment a falnak");
            jatekmanager.Instance.UpdateGameState(jatekmanager.GameState.Meghaltal);
            /*#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif*/
            
        }
    }
}
