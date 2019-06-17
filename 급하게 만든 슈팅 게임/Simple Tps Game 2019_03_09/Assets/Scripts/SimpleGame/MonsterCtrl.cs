using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterCtrl : MonoBehaviour {

    //몬스터를 위한 전체적인 함수들 모음(?)
    [System.Serializable]
    public class MonsterSetting {
        [Header(" - Monster Setting")]
        public float damage = 15.0f;
        public float speed = 10.0f;
        public float hp = 100.0f;
        public float minScore = 5.0f;
        public float maxScore = 10.0f;
        public GameObject money;
    }
    [SerializeField]
    public MonsterSetting monsterSetting;

    //참조가 필요하다고 하여...
    public GameMgr gameMgr;

    //플레이어 추적을 위함
    public NavMeshAgent navAgent;
    public Transform Target;

    //Monster Hp
    public GameObject showHp;
    public Slider hp;
    

	// Use this for initialization
	void Start () {
        //필요한 컴포넌트 추가, 초기화
        navAgent = GetComponent<NavMeshAgent>();
        Target = FindObjectOfType<PlayerCtrl>().transform;
        gameMgr = FindObjectOfType<GameMgr>().GetComponent<GameMgr>();
        navAgent.speed = monsterSetting.speed;
        hp.maxValue = monsterSetting.hp;
        hp.value = monsterSetting.hp;
        showHp.SetActive(false);
    }
	
	void Update () {
        //추적 활성화
        navAgent.destination = Target.position;

        //몬스터의 hp HUD가 카메라를 보도록 설정
        showHp.GetComponent<Transform>().LookAt(Camera.main.transform);

        //공격을 받기 시작하면 hp 보여줌
        if (monsterSetting.hp < hp.GetComponent<Slider>().maxValue)
            showHp.SetActive(true);

        hp.value = monsterSetting.hp;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            //hp를 충돌한 오브젝트의 데미지 만큼 감소
            monsterSetting.hp -= coll.gameObject.GetComponent<BulletCtrl>().currentDamage;
            //충돌한 후 관통 현상을 없애기 위해 삭제
            Destroy(coll.gameObject, 0.0f);

            //충돌후 hp가 0보다 적을때
            if (monsterSetting.hp <= 0.0f)
            {
                gameMgr.roundCtrl.score += Random.Range(monsterSetting.minScore, monsterSetting.maxScore);
                gameMgr.roundCtrl.killCount++;
                //gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(this.gameObject, 0.0f);
                Instantiate(monsterSetting.money, transform.position, transform.rotation);
            }
        }
    }
}
