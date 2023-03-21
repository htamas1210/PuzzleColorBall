using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUIManager : MonoBehaviour
{
    public void LoadMenuScene() {
        SceneManager.LoadScene(0);
    }

    public void LoadPauseMenu() {
        //to be implemented when the ui is done
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
    
}