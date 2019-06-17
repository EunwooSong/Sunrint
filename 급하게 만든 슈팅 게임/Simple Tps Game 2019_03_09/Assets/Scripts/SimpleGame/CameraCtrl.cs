using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    public Transform target;
    public float smoothing = 5.0f;  //약간의 지연

    Vector3 offset;                 //플레이어와 카메라 오프셋 보관하여 동일한 오프셋을 유지하기 위함

	// Use this for initialization
	void Start () {
        target = FindObjectOfType<PlayerCtrl>().transform;  //Player을 자동으로 찾아 저장
        offset = transform.position - target.position;      //카메라에서 플레이어까지의 위치
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
