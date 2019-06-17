using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    //Move Setting---------------------------------------------------------
    [System.Serializable]
    public class MovementSetting
    {
        public float moveSpeed = 5.0f;
        public float rotSpeed = 100.0f;
        public float smoothRot = 8.0f;
    }
    [SerializeField]
    public MovementSetting movement;

    Vector3 MoveDir;
    Transform tr;
    CharacterController ch;

    //Rot Setting(Cameara)-------------------------------------------------
    int floorMask;
    float rayLength = 100.0f;

    //FlashLight-----------------------------------------------------------
    public bool isFlashLight;
    public GameObject flashLight;

    //Hp, Sprint, Money----------------------------------------------------
    public float hp;
    public float sprint;
    public GameMgr gameMgr;


    // Use this for initialization
    void Start()
    {
        floorMask = LayerMask.GetMask("Floor"); //Floor Mask 번호 저장

        ch = GetComponent<CharacterController>();
        tr = GetComponent<Transform>();

        gameMgr = FindObjectOfType<GameMgr>().GetComponent<GameMgr>();

        hp = 100.0f;
        sprint = 100.0f;
        isFlashLight = false;
        StartCoroutine("HpAndSprintCtrl");
    }

    // Update is called once per frame
    void Update()
    {
        //샵 사용중일때 이동 X, 죽었을 때 이동 X
        if (!gameMgr.isUseShop || !gameMgr.roundCtrl.isPlayerDied)
        {
            FlashLightCtrl();   //조명 조정
                                //이동-------------------------------------------------------------
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            MoveDir = transform.TransformDirection(MoveDir);        //월드 공간 기준으로 변경

            //달리기
            if (Input.GetKey(KeyCode.LeftShift) && sprint > 0.0f)
            {
                StartCoroutine("SprintCtrl");
                MoveDir *= movement.moveSpeed * 2;
            }
            else MoveDir *= movement.moveSpeed;

            ViewControl();
        }
        //최종 이동--------------------------------------------------------
        ch.Move(MoveDir * Time.deltaTime);
        //Hp, Sprint 값 표시-----------------------------------------------
        GameMgr.instance.PlayerData(hp, sprint);
        
        //죽음을 감지하는 스크립트
        CheckDied();
    }

    void CheckDied() {
        if (hp <= 0.0f)
        {
            gameMgr.roundCtrl.isPlayerDied = true;
            movement.moveSpeed = 0.0f;
            gameObject.GetComponent<PlayerCtrl>().enabled = false;  //더이상 조작 금지!!
        }
    }

    //손전등(?) 관리 함수
    void FlashLightCtrl()
    {
        if (Input.GetKeyDown(KeyCode.F)) isFlashLight = !isFlashLight;
        flashLight.SetActive(isFlashLight);
    }

    //마우스 위치에서 빔을쏴, 빔이 맞은 곳의 위치값을 얻어 플레이어를 회전하도록 제작
    void ViewControl()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        //빔을 쏴 그곳의 백터값 확인
        if (Physics.Raycast(camRay, out floorHit, rayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0.0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            tr.rotation = newRotation;  //플레이어 회전
        }
    }

    //HP, Sprint 코루틴
    IEnumerator HpAndSprintCtrl()
    {
        //죽지 않았을때 Hp와  Sprint 증가
        while (!gameMgr.roundCtrl.isPlayerDied)
        {
            //0.001초마다 hp는 0.005만큼, sprint는 0.03만큼 증가
            yield return new WaitForSeconds(0.001f);
            if (hp < 100.0f) hp += 0.005f;
            if (sprint < 100.0f) sprint += 0.03f;
        }
    }

    IEnumerator SprintCtrl()
    {
        //0.001초마다 감소
        yield return new WaitForSeconds(0.001f);
        sprint -= 0.2f;
    }

    //충돌감지 스크립트
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag.Equals("Money"))
        {
            gameMgr.roundCtrl.money += coll.gameObject.GetComponent<CoinCtrl>().money;
            Destroy(coll.gameObject, 0.0f);
        }

        if (coll.gameObject.tag.Equals("Monster"))
        {
            //몬스터와 부딪치면 데미지 감소
            hp -= coll.gameObject.GetComponent<MonsterCtrl>().monsterSetting.damage;
        }
    }
}
