using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{

    //싱글톤을 사용하여 다양한 스크립트에서 사용이 가능하도록 함
    public static GameMgr instance = null;

    //게임 UI 관련
    [System.Serializable]
    public class GameUI
    {
        [Header(" - Bullet Text Setting")]
        public Text currentBullet;      //현재 총알
        public Text maximumBullet;      //최대 총알
        public Text gunName;            //총이름
        public GameObject isReLoading;  //재장전중일때 활성화

        [Header(" - Player Text Setting")]
        public Slider hp;       //체력
        public Slider sprint;   //전력 질주

        [Header(" - Start and Died Text")]
        public GameObject start;
        public GameObject died;
        public string[] fade;       // 0 -> in, 1 -> out

        [Header(" - Died Text Setting")]
        public Text finalScore;
        public Text finalKillCount;
        public GameObject finishPanel;

        [Header(" - Game Play Text Setting")]
        public GameObject round;          //라운드가 시작될 때 마다 보여줄 Text
        public Text score;          //현재까지 얻은 스코어 보여줄 Text
        public Text money;          //상점에서 사용가능한 돈을 보여줄 Text
        public Text killCount;      //현재까지 킬한 몬스터 수를 보여줄 Text
        public Text totalMonster;   //스폰된 몬스터 수를 보여줄 text
        public GameObject breakTime;      //라운드가 종료됨을 보여줄 text

        [Header(" - Shop Text Setting")]
        public GameObject shopUI;
        public Text UpGunName;
        public Text bulletLimit;
        public Text damage;
        public GameObject moneyLack;
    }
    [SerializeField]
    public GameUI gameUI;

    //게임 라운드? 관련
    [System.Serializable]
    public class RoundCtrl 
    {
        [Header(" - Monster Ctrl")]
        public GameObject[] monster;    // 0 -> easy, 1 -> Nomal, 2 -> Hard
        public Transform[] spawnPoint;
        public int[] monsterAmount;     // 0 -> easyAmount, 1-> NomalAmount, 2 -> HardAmount

        [Header(" - Round Ctrl")]
        public GameObject difficulty;
        public int round;               //현재 라운드
        public int killCount;           //죽인 몬스터의 수
        public float breakTime;
        public int totalMonster;        //라운드가 종료 되었는지 확인을 위함
        public bool roundStart;         //라운드가 시작될 때만 검사하기를 위함  
        
        public int nomalMode = 5;         //노말 난이도가 시작되는 난이도
        public int hardMode = 10;          //하드 난이도가 시작되는 난이도

        [Header(" - Player Ctrl")]
        public float score;             //스코어
        public int money;               //돈
        public bool isPlayerDied;       //플레이어가 죽을시 사용할 함수

    }

    [SerializeField]
    public RoundCtrl roundCtrl;

    //Shop
    [Header(" - For Shop")]
    public bool isUseShop;
    public GunCtrl gunInfo;

    void Awake()
    {
        instance = this;    //다른 코드에서 GameMgr함수등이 사용이 가능하도록 제작
    }

    void Start()
    {
        roundCtrl.roundStart = false;
        StartCoroutine("StartGame");    //게임 시작 코루틴을 사용함으로써 게임 시작
    }

    void Update()
    {
        //만약 난이도가 5이상이면 저녁쯤으로, 10이상이면 밤으로 변경
        if (roundCtrl.round >= roundCtrl.hardMode)
            roundCtrl.difficulty.GetComponent<Transform>().rotation = Quaternion.Lerp(roundCtrl.difficulty.GetComponent<Transform>().rotation, Quaternion.Euler(200, 20, -8), 0.005f);

        else if (roundCtrl.round >= roundCtrl.nomalMode)
            roundCtrl.difficulty.GetComponent<Transform>().rotation = Quaternion.Lerp(roundCtrl.difficulty.GetComponent<Transform>().rotation, Quaternion.Euler(170, 20, -8), 0.005f);

        else
            roundCtrl.difficulty.GetComponent<Transform>().rotation = Quaternion.Lerp(roundCtrl.difficulty.GetComponent<Transform>().rotation, Quaternion.Euler(120, 20, -8), 0.005f);


        //게임 UI 보여줌
        ShowGameUI();

        //게임이 정상 시작될 때 까지 실행 x
        if (roundCtrl.killCount == roundCtrl.totalMonster && roundCtrl.roundStart)
            StartCoroutine("StartRound");    //죽인 킬수가 생성된 몬스터 수와 같을때 다음 라운드로 진행

        //샵을 사용하는지 체크
        ShopManager();

        if (roundCtrl.isPlayerDied) {
            //StopAllCoroutines(); //모든 코루틴 종료
            StartCoroutine("PlayerDied");
        }
    }

    IEnumerator PlayerDied()
    {
        gameUI.died.SetActive(true);
        gameUI.died.GetComponent<Animation>().Play(gameUI.fade[0]);
        yield return new WaitForSeconds(3.0f);
        gameUI.died.GetComponent<Animation>().Play(gameUI.fade[1]);
        yield return new WaitForSeconds(1.5f);

        gameUI.finalScore.text = "You Get " + roundCtrl.score.ToString("N1") + "score!!";
        gameUI.finalKillCount.text = "Kill Count : " + roundCtrl.killCount.ToString();
        gameUI.finishPanel.SetActive(true);
    }

    //몬스터 관련 함수 --------------------------------------------------------------------------------
    //게임 시작 코루틴
    IEnumerator StartGame()
    {
        //시작을 알려주는 Text 에니메이션 시작
        gameUI.start.SetActive(true);
        gameUI.start.GetComponent<Animation>().Play(gameUI.fade[0]);
        yield return new WaitForSeconds(3.0f);
        gameUI.start.GetComponent<Animation>().Play(gameUI.fade[1]);

        //RoundCtrl로 넘어가 몬스터 생성에 관한 스크립트 활성화
        StartCoroutine("StartRound");
    }

    //매 라운드마다 실행할 코루틴
    IEnumerator StartRound()
    {
        //Update에서 라운드가 시작될 때만 검사가 가능하도록 제한(?)
        roundCtrl.roundStart = false;

        //라운드를 증가시킴
        roundCtrl.round++;

        //전체조명에 문제가 있어 먼저 실행
        yield return new WaitForSeconds(1.0f);
        Hard();

        //처음시작하는 게임일때 BreakTime 없음
        if (roundCtrl.score == 1)
            yield return null;

        else
            yield return new WaitForSeconds(roundCtrl.breakTime);

        //라운드가 지남에 따라 난이도 조절
        Easy(); Nomal();

        //totalMonster에 생성된 몬스터수 라운드 마다 저장
        foreach (int monster in roundCtrl.monsterAmount)
            roundCtrl.totalMonster += monster;

        roundCtrl.roundStart = true;

        //현재 라운드를 플레이어에게 보여준후, 게임 시작
        gameUI.round.GetComponent<Text>().text = "Round " + roundCtrl.round;
        gameUI.round.SetActive(true);
        gameUI.round.GetComponent<Animation>().Play(gameUI.fade[0]);
        yield return new WaitForSeconds(3.0f);
        gameUI.round.GetComponent<Animation>().Play(gameUI.fade[1]);

        //모든 과정을 끝낸 후, 몬스터를 생성하여 다음 라운드 시작
        StartCoroutine("SpawnMonster");
    }

    //Easy 난이도 몬스터 수 설정
    void Easy()
    {
        roundCtrl.monsterAmount[0] = roundCtrl.round * 2; //매 라운드 마다 2명씩 증가
    }

    //Nomal 난이도 몬스터 수 설정
    void Nomal()
    {
        if (roundCtrl.round >= roundCtrl.nomalMode)
            roundCtrl.monsterAmount[1] += 1; //5 라운드 이후 몬스터 1씩 증가
    }

    //Hard 난이도 몬스터 수 설정
    void Hard()
    {
        if (roundCtrl.round >= roundCtrl.hardMode)
        {
            roundCtrl.monsterAmount[2] += 1; //10 라운드 이후 몬스터 1씩 증가
            roundCtrl.difficulty.SetActive(false);
        }
    }

    //몬스터 생성
    IEnumerator SpawnMonster()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < roundCtrl.monsterAmount[j]; i++)
            {
                //1초 간격으로 몬스터를 랜덤한 위치에 정해진 갯수 만큼 생성
                yield return new WaitForSeconds(1f);
                int spawnPoint = (int)Random.Range(0.0f, (float)roundCtrl.spawnPoint.Length);
                Instantiate(roundCtrl.monster[j], roundCtrl.spawnPoint[spawnPoint].position, roundCtrl.spawnPoint[spawnPoint].rotation);
            }
        }

    }

    //UI 관련 함수------------------------------------------------------------------------------------

    //Player UI
    public void PlayerData(float hp, float sprint)
    {
        gameUI.hp.value = hp;
        gameUI.sprint.value = sprint;
    }

    //Gun UI 
    public void BulletText(int currentBullet, int maximumBullet, string gunName, bool isReload, bool isinfinite)
    {
        //총 이름, 탄환 등 바꿈
        gameUI.currentBullet.text = "" + currentBullet;
        if (!isinfinite)
            gameUI.maximumBullet.text = "" + maximumBullet;
        else
            gameUI.maximumBullet.text = "Infinite";
        gameUI.gunName.text = gunName;
        gameUI.isReLoading.SetActive(isReload);
    }

    //GameUI
    void ShowGameUI() {
        gameUI.breakTime.SetActive(!roundCtrl.roundStart);
        gameUI.score.text = "Score : " + roundCtrl.score.ToString("N1");
        gameUI.money.text = "Money : " + roundCtrl.money;
        gameUI.killCount.text = roundCtrl.killCount.ToString();
        gameUI.totalMonster.text = roundCtrl.totalMonster.ToString();
    }

    //ShopCtrl
    void ShopManager() {
        //샵의 사용여부 확인
        if (Input.GetKeyDown(KeyCode.I)) isUseShop = !isUseShop;

        //플레이어가 샵을 사용할때 정보갱신
        gameUI.shopUI.SetActive(isUseShop);

        if (isUseShop)
        {
            gunInfo = FindObjectOfType<WeaponsHolder>().currentWeapon;
            gameUI.UpGunName.text = gunInfo.gunSetting.gunName;
            gameUI.bulletLimit.text = "Bullet Limit : " + gunInfo.gunSetting.BulletLimit.ToString();
            gameUI.damage.text = "Damage : " + gunInfo.gunSetting.damage.ToString();
        }
    }

    //탄창크기 증가
    public void UpgradeBulletLimit() {
        if (roundCtrl.money - 1500 >= 0)
        {
            gunInfo.gunSetting.BulletLimit += 8;
            roundCtrl.money -= 1500;
        }
        else
        {
            StartCoroutine("MoneyLack");
        }
    }

    //데미지 증가
    public void UpgradeDamage() {
        if (roundCtrl.money - 1500 >= 0)
        {
            gunInfo.gunSetting.damage += 5;
            roundCtrl.money -= 1500;
        }
        else
        {
            StartCoroutine("MoneyLack");
        }
    }

    //총알 구매
    public void AddBullet() {
        if (roundCtrl.money - 100 >= 0)
        {
            gunInfo.gunSetting.maximumBullet += 100;
            roundCtrl.money -= 100;
        }
        else
        {
            StartCoroutine("MoneyLack");
        }
    }

    //게임 화면 클릭시 샵 종료함수
    public void ExitShop() {
        isUseShop = !isUseShop;
    }

    IEnumerator MoneyLack() {
        gameUI.moneyLack.SetActive(true);
        gameUI.moneyLack.GetComponent<Animation>().Play(gameUI.fade[1]);
        yield return new WaitForSeconds(3.0f);
        gameUI.moneyLack.SetActive(false);
    }
}
