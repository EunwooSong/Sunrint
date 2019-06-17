using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    //Text UI 항목 연결을 위한 변수
    public Text txtScore;
    //누적 점수를 기록하기 위한 변수
    private int totScore = 0;
    // Use this for initialization
    void Start()
    {
        //스코어 불러오기
        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        DispScore(0);
    }
    public void DispScore(int score)
    {
        totScore += score;
        txtScore.text = "GOLD " + totScore.ToString();

        //스코어 저장
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
    }
}
