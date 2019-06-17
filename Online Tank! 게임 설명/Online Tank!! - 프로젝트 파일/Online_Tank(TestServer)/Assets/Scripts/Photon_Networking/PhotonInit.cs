using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInit : MonoBehaviour {


    public string version = "0.0.1";    //과거 버전과 멀티 제한을 위함

    //포톤 클라우드 접속
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(version);
    }

    //포톤 클라우드에 정상적으로 접속 -> 로비 입장하면 호출 
    void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    //무작위 룸 접속에 실패
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms");
        PhotonNetwork.CreateRoom ("MyRoom");    //방 생성
    }

    //룸에 입장, 탱크 생성
    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");
        CreateTank();
    }

    //접속자의 탱크 생성
    void CreateTank()
    {
        float pos = Random.Range(-20.0f, 20.0f);    //탱크 생성 위치 무작위
        PhotonNetwork.Instantiate("Player", new Vector3(pos, 5.0f, pos), Quaternion.identity, 0);
    }

    //좌측 상단에 접속 과정에 대한 로그를 출력
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
