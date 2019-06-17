using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : MonoBehaviour {


    [System.Serializable]
    public class GunSetting {
        [Header(" - Gun Name")]
        public string gunName = "GUN";

        [Header(" - Bullet")]
        public int maximumBullet = 240;
        public int currentBullet = 10;
        public int BulletLimit = 30;
        public bool isinfinite;

        [Header(" - Damage Setting")]
        public float damage = 20.0f;

        [Header(" - Delay Setting")]
        public float shotDelay = 0.5f;
        public float reloadTime = 3.0f;
        public bool isShot;
        public bool isReload;

        [Header(" - Repeater")]
        public bool isCanRepeater;

        [Header(" - Rebound Setting")]
        public float duration = 0.45f;
        public float power = 0.15f;
    }
    [SerializeField]
    public GunSetting gunSetting;

    public enum GunMode {
        Single, Repeater
    }
    public GunMode gunState;

    public GameObject bullet;
    public Transform pivot;

    //총 반동
    public CameraShake cameraShake;

	// Use this for initialization
	void Start () {
        gunState = GunMode.Single;
        BulletCtrl.Damage = gunSetting.damage;
        gunSetting.isShot = false;
        gunSetting.isReload = false;
        gunSetting.currentBullet = gunSetting.BulletLimit;
        cameraShake = Camera.main.GetComponent<CameraShake>();  //메인 카메라의 CameraShake 컴포넌트 불러오기
    }
	
	// Update is called once per frame
	void Update () {
        //UI에 발사 가능한 총알, 최대 총알, 총 이름 출력
        GameMgr.instance.BulletText(gunSetting.currentBullet, gunSetting.maximumBullet, gunSetting.gunName, gunSetting.isReload, gunSetting.isinfinite);

        //샵 사용중일때 사격 X, 죽었을때 사격 x
        if (!GameMgr.instance.isUseShop && !GameMgr.instance.roundCtrl.isPlayerDied)
        {
            //단발, 연발 전환
            if (Input.GetKeyDown(KeyCode.B) && gunSetting.isCanRepeater)
                switch (gunState)
                {
                    case GunMode.Single:
                        gunState = GunMode.Repeater;    //연발로 전환
                        break;
                    case GunMode.Repeater:
                        gunState = GunMode.Single;      //단발로 전환
                        break;
                }

            //탄창 가득? x [And] R키 누름? -> 재장전
            if (Input.GetKeyDown(KeyCode.R) && gunSetting.currentBullet != gunSetting.BulletLimit)
            {
                gunSetting.isReload = true;
                StartCoroutine("ReLoad");       //제장전 시작
            }

            //딜레이
            if (!gunSetting.isShot && !gunSetting.isReload)
            {
                //발사 - 단발
                if (gunState == GunMode.Single)
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine("Shot");
                    }

                //발사 - 연발
                if (gunState == GunMode.Repeater)
                {
                    if (Input.GetMouseButton(0))
                    {
                        StartCoroutine("Shot");
                    }
                }


            }
        }
    }

    //발사
    IEnumerator Shot() {
        if (gunSetting.currentBullet != 0) //장전된 탄이 0이 아닐때 발사 가능
        {
            gunSetting.isShot = true;
            Instantiate(bullet, pivot.position, pivot.rotation);//총알 생성
            gunSetting.currentBullet--;                         //장전된 총알 감소
            cameraShake.StartCoroutine(cameraShake.Shake(gunSetting.power, gunSetting.duration));  //카메라 반동 효과 활성화
        }

        yield return new WaitForSeconds(gunSetting.shotDelay);  //발사에 지연을 줌

        gunSetting.isShot = false;
    }

    //재장전
    IEnumerator ReLoad() {

        gunSetting.isReload = true;                             //재장전중임을 알려줘, 발사에 제한을 줌

        yield return new WaitForSeconds(gunSetting.reloadTime);
        //재장전
        if (!gunSetting.isinfinite)
        {
            if (gunSetting.maximumBullet - (gunSetting.BulletLimit - gunSetting.currentBullet) > 0)
            {
                gunSetting.maximumBullet -= gunSetting.BulletLimit - gunSetting.currentBullet;
                gunSetting.currentBullet = gunSetting.BulletLimit;
            }
            else
            {
                gunSetting.currentBullet = gunSetting.maximumBullet;
                gunSetting.maximumBullet = 0;
            }
        }
        else
            gunSetting.currentBullet = gunSetting.BulletLimit;

        gunSetting.isReload = false;
    }
}
