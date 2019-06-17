using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnetGunCtrl : MonoBehaviour {

    // Use this for initialization
    // Creat Bullet(9mm), Sound, Effect . . .
    //public GameObject bullet;
    //public GameObject emptyCartridge;
    //public Transform firePos;
    //public Transform aimPoint;
    //private float y = 0.0f;
    //private float mouseYSpeed = 5.0f;

    //public AudioClip fireSound;
    //public GameObject fireEffect;

    //카메라의 에임이 총의 Y축이 같아야 함
    //마우스 0 or 1 누를시 작동, 그 전에는 에니메이션 + 나중에 에니메이션 처리까지

	void Start () {
        //Vector3 Angles = transform.eulerAngles;
       // y = Angles.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            CreateBullet();
            CreateSound();
            Debug.Log("Bang");
        }
	}

    void CreateSound() {
        //소리 생성?
        Debug.Log("BANG!!!");
    }

    void CreateBullet() {
        //총 발사
        //Instantiate(bullet, firePos.position, firePos.rotation);
    }
}
