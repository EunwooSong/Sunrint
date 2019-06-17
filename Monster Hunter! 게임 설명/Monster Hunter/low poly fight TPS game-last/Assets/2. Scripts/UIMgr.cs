using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIMgr : MonoBehaviour {

    //게임 화면으로 이동
    public void OnClickStartBtn()
    {
        Debug.Log("Click Button");
        SceneManager.LoadScene("MainScene backup");
    }
}
