using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mConnonCtrl : MonoBehaviour {

    //포탄 속도
    public float Speed = 5000.0f;

    //폭발 효과
    public GameObject expEffect;

    private SphereCollider sCollider;   //충돌 감지를 위함
    private Rigidbody _rigidbody;       //중력 적용을 위함

	// Use this for initialization
	void Start ()
    {
        sCollider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().AddForce(transform.forward * Speed);  //생성과 동시에 앞으로 포 나감

        StartCoroutine(this.Explosion(3.0f));   //3초뒤 폭발
	}

    void OnTriggerEnter()
    {
        StartCoroutine(this.Explosion(0.0f));   //충돌이 생기면 바로 폭발
    }

    IEnumerator Explosion(float time)
    {
        yield return new WaitForSeconds(time);
        sCollider.enabled = false;      //충돌감지 종료
        _rigidbody.isKinematic = true;  //물리엔진 사용 종료  

        GameObject obj = (GameObject)Instantiate(expEffect, transform.position, Quaternion.identity);   //폭발 효과 생성

        Destroy(obj, 1.0f); //폭발효과 제거

        Destroy(this.gameObject, 1.0f); //포탄 제거
    }
}
