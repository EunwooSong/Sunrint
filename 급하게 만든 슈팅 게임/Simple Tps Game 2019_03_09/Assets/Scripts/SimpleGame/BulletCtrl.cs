using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    public static float Damage;
    public float currentDamage;
    public float speed = 6000.0f;

    void Start()
    {
        currentDamage = Damage;
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        StartCoroutine("DamageCtrl");    //발사된 시간이 지남에 따라 데미지 감소
        StartCoroutine("DestoryBullet");    //발사 후 3초뒤 필요없는 오브젝트 제거
	}

    IEnumerator DamageCtrl() {
        yield return new WaitForSeconds(0.01f);
        currentDamage -= 0.02f;
    }

    IEnumerator DestoryBullet() {
        yield return new WaitForSeconds(3.0f);
        currentDamage = Damage;
        Destroy(this.gameObject, 0.0f);
    }
}
