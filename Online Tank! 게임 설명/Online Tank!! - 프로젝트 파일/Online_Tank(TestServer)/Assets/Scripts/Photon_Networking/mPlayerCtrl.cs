using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class mPlayerCtrl : MonoBehaviour {

    private Transform tr;
    private CharacterController ch;

    public float MSpeed = 5.0f;
    public float RSpeed = 130.0f;

    private float h = 0.0f;
    private float v = 0.0f;

    private Vector3 Move = Vector3.zero;

    private PhotonView pv = null;
    public Transform CamPivot;

    //부드러운 이동, 회전
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

	// Use this for initialization
	void Start ()
    {
        tr = GetComponent<Transform>();
        ch = GetComponent<CharacterController>();

        pv = GetComponent<PhotonView>();
        //데이터 전송 타입울 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        //PhotonView Observed Components 속성에 mPlayerCtrl 스크립트를 연결
        pv.ObservedComponents[0] = this;


        //내가 움직이는 탱크가 맞을때 카메라 타겟 자동 설정
        if (pv.isMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = CamPivot;
        }

        currPos = tr.position;
        currRot = tr.rotation;
	}

    //데이터 전송, 받음 관리
    void OnPhotonSerializeView(PhotonStream _pStream, PhotonMessageInfo _pInfo)
    {
        if (_pStream.isWriting)
        {
            _pStream.SendNext(tr.position); //포지션 값 전송
            _pStream.SendNext(tr.rotation); //회전 값 전송
        }
        else
        {
            currPos = (Vector3)_pStream.ReceiveNext();      //포지션 값 받음
            currRot = (Quaternion)_pStream.ReceiveNext();   //회전 값 받음
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //자신의 탱크 이동
        if (pv.isMine)
        {
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");

            tr.Rotate(Vector3.up * h * RSpeed * Time.deltaTime);

            Move = (tr.forward * v) + (tr.right * h);

            Move.y -= 100f * Time.deltaTime;

            ch.Move(Move * MSpeed * Time.deltaTime);//플레이어 이동!
        }
        //원격 플레이어 이동(부드럽게!!)
        else
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3.0f);
        }
	}
}
