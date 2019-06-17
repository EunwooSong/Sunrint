using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {

    public void Btn_MainMenu(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }


    public void Btn_StartGame(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void Btn_ExitGame() {
        Debug.Log("ExitGame!!");
        Application.Quit();
    }
}
