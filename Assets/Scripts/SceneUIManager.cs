using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUIManager : MonoBehaviour
{
    private int menuSceneIndex = 0, currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    public void LoadMenuScene() {
        SceneManager.LoadScene(menuSceneIndex);
    }

    public void LoadPauseMenu() {
        //to be implemented when the ui is done
    }

    public void LoadOptionsMenu() {
        //to be implemented when the ui is done
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}