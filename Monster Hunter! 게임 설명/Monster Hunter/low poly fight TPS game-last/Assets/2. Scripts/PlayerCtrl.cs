using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
    private float v = 0.0f;         //앞, 뒤 이동
    private float m = 0.0f;         //메테오 발동 변수
    private Transform tr;
    public float moveSpeed = 1.0f;  //이동 속도
    public float nextWield = 0.0f;  //공격 딜레이
    public float wieldRate = 0.6f;  
    public float rotSpeed = 100.0f; //회전 속도
    public float nextMeteor = 0.0f; //스킬 딜레이
	public float mpup = 0.0f;       //mp 증가를 위함
    public float MeteorFireRate = 15.0f;
    public GameObject meteor;       //meteor 오브젝트
    public GameObject sw;           //칼 충돌 처리 오브젝트
    public Transform swp;           //sw 오브젝트 생성 위치
	public GameObject youDiePrefab; //죽음을 알려주는 프리팹
	public GameObject reGamePrefab; //메인화면으로 돌아가는 프리팹

    Animator _anim;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    public int hp = 100;
    public float mp = 100.0f;

    private int initHP;
    public Image imgHpbar;
	private float initMP;
    public Image imgMpbar;
	private float coolTime;
	public Image imgCoolTime;

    // Use this for initialization
    void Start () {
        initHP = hp;
        initMP = mp;
        tr = GetComponent<Transform>();
        _anim = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {
        v = Input.GetAxis("Vertical");      //이동 값 받기
        m = Input.GetAxisRaw("Fire3");      //스킬 값 받기
        tr.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime, Space.Self); //이동
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));   //회전
        _anim.SetFloat("Speed", v); //에니메이션 바꿔줌

        //기본 공격
		if (Input.GetMouseButtonDown (0) && Time.time > nextWield)
        {
            nextWield = Time.time + wieldRate;
            StartCoroutine(this.yourCh());
        }

        //메테오 컨트롤
        if (m>0.1f && Time.time > nextMeteor)
        {
            nextMeteor = Time.time + MeteorFireRate;
            StartCoroutine(this.MeteorFire());
        }

        //mp 컨트롤
		if (mp <= 100.0f && Time.time > mpup)
		{
			mpup = Time.time + 0.1f;
			imgMpbar.fillAmount += (float)0.1f / (float)initMP;
			mp += 0.1f;
		}

        //쿨타임 컨트롤
		if (imgCoolTime.fillAmount != 1.0f && Time.time > coolTime)
		{
			coolTime = Time.time + 0.2f;
			imgCoolTime.fillAmount += (float)0.2f / (float)MeteorFireRate;
		}
    }

    IEnumerator MeteorFire()
    {
		moveSpeed = 0.0f;
        yield return new WaitForSeconds(2.0f);
        if(mp > 0)
        {
			imgCoolTime.fillAmount = 0.0f;
			Instantiate(meteor, swp.position, swp.rotation);
            mp -= 25;
        }
		moveSpeed = 5.0f;
        imgMpbar.fillAmount = (float)mp / (float)initMP;
        Debug.Log("Player MP = " + mp.ToString());
        
    }
    
	IEnumerator yourCh()
	{
        _anim.SetTrigger("Attack");
        if (hp > 0)
        {
            Instantiate(sw, swp.position, swp.rotation);
        }
		yield return new WaitForSeconds (0.3f);
    }

    //공격을 받았을 떄
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PUNCH") //충돌한 오브젝트의 tag가 "PUNCH"일때 HP 감소
        {
            hp -= 10;
            imgHpbar.fillAmount = (float)hp / (float)initHP;
            Debug.Log("Player HP = " + hp.ToString());

            if(hp <= 0)
            {
                PlayerDie();
                
            }
        }
    }

    //죽었을때 델리게이트를 이용하여 몬스터에게 게임 종료를 알려줌, 죽음을 화면에 띄우고 메인화면으로 돌아가는 버튼 생성 
    void PlayerDie()
    {
        Debug.Log("Player Die !!");
        _anim.SetTrigger("DiePlayer");
        /*
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach(GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
        */
        OnPlayerDie();
        moveSpeed = 0.0f;
        rotSpeed = 0.0f;
        GameMgr.instance.isGameOver = true;
		Instantiate (youDiePrefab, swp.position, swp.rotation);
		Destroy (youDiePrefab, 3.0f);
		Instantiate (reGamePrefab, swp.position, swp.rotation);
    }
}
