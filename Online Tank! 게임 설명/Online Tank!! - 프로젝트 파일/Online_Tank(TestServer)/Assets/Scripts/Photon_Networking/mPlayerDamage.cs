using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPlayerDamage : MonoBehaviour {

    private MeshRenderer[] render;          //탱크 폭발시 비활성화를 위함

    private GameObject expEffect = null;    //폭발 효과

    public int Hp = 100;        //탱크의 최대 Hp

    private int currHp = 0;     //탱크의 현재 Hp

	// Use this for initialization
	void Start ()
    {
        render = GetComponentsInChildren<MeshRenderer>();

        currHp = Hp;    //Hp 설정

        expEffect = Resources.Load<GameObject>("expEffect_L");  //자동으로 폭발 효과 불러옴
	}

    void OnTriggerEnter(Collider coll)
    {
        if (currHp > 0 && coll.GetComponent<Collider>().tag == "CANNON")    //충돌한 오브젝트의 tag가 CANNON, HP가 0보다 클때 실행
        {
            currHp -= 20;

            //현제 Hp가 0보다 작거나 같을때 폭발
            if (currHp <= 0)
            {
                StartCoroutine(this.Explosion());   //코루틴 실행
            }
        }
    }

    IEnumerator Explosion()
    {
        Object effect = GameObject.Instantiate(expEffect, transform.position, Quaternion.identity); //폭발 효과 생성
           
        Destroy(effect, 3.0f);  //포발 효과 제거

        SetPlayerVisible(false);

        yield return new WaitForSeconds(5.0f);  //5초동안 플레이어 숨김

        currHp = Hp;    //현재 Hp 다시 설정
        SetPlayerVisible(true);
    }

    void SetPlayerVisible(bool isVisible)
    {
        foreach (MeshRenderer _render in render)
        {
            _render.enabled = isVisible;    //탱크 렌더러 활성화, 비활성화 관리
        }
    }
}
