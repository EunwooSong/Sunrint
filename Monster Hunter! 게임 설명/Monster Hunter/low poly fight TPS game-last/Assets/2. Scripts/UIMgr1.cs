using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIMgr1 : MonoBehaviour {
    
    //메인화면으로 이동
    public void OnClickStartBtn()
    {
        Debug.Log("ReStart!");
        SceneManager.LoadScene("MS.Scene");
    }
}
