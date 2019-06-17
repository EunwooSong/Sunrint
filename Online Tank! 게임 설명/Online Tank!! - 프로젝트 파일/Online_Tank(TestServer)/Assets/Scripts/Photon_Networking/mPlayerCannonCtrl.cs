using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPlayerCannonCtrl : MonoBehaviour {

    private GameObject cannon = null;   //포탄 오브젝트

    public Transform firePosition;      //포탄 발사 위치

    private PhotonView pv = null;

	// Use this for initialization
	void Start ()
    {
        cannon = (GameObject)Resources.Load("Cannon");  //Cannon을 자동으로 불러옴
        pv = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (pv.isMine && Input.GetMouseButtonDown(0))
        {
            Fire();                                         //자신의 탱크가 포 발사
            pv.RPC("Fire", PhotonTargets.Others, null);     //원격 플레이어 탱크가 발사
            //Void PhotonView.RPC(string methodName, PhotonPlayer target, params object[] parameters)
        }
	}

    [PunRPC]
    void Fire()
    {
        Instantiate(cannon, firePosition.position, firePosition.rotation);  //포탄을 발사위치에 생성시킴
    }
}
