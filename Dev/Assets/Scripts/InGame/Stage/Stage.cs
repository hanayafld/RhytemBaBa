using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    #region Action
    public System.Action OnStageClear;//스테이지 클리어시
    public System.Action OnStageRestart;//스테이시 다시시작 시
    public System.Action OnStageFailed;//스테이지 실패시

    private System.Action OnStartGame;//카운트 다운 끝나면
    #endregion

    private HeroInfo heroinfo;
    private StageData stageData;

    #region UI
    public Image img_dim;
    public Image img_UIOutLine;
    public Text txt_countDown;//카운트 다운 텍스트
    public Text txt_startGame;//게임 시작! 알림 텍스트
    #endregion

    #region 게임 진행
    public Hero hero;
    private List<Monster> stageMonsters;
    private Monster currentMonster;//현제 전투중인 몬스터
    private StagePrefab stagePrefab;

    private int bitCount;//0~15 // 비트 카운트 홀수는 엇박
    private int stageProgress;//0:스테이지 시작 1:스테이지 클리어 2:보스클리어
    private bool spawnMonster;

    #region Note
    public Goal perfectZone;
    public Goal missZone;
    public Note[] notes;
    #endregion

    #endregion


    void Awake()
    {
        this.stageMonsters = new List<Monster>();
    }

    public void Init(HeroInfo heroInfo)
    {
        Debug.Log("Stage Init");
        //인포 불러오기
        this.heroinfo = heroInfo;

        //데이터 불러오기(스테이지 데이터와 몬스터 데이터)
        DataManager.Instance.LoadAllData();
        this.stageData = DataManager.Instance.dicStageData[this.heroinfo.stageLevel];

        this.LoadStagePrefab();//스테이지 프리팹 불러오기
        this.Ready2Monster();//몬스터들 불러오기

        this.stagePrefab.OnTick = () =>
        {
            this.OnTick();//스테이지에서 온틱 넘어올때마다, 여기서 온 틱 호출
        };
        this.stagePrefab.OnTimerEnd = () =>
        {
            this.TimerEnd();//노래 시간 끝
            this.stageProgress++;
            if (this.stageProgress == 1)
            {
                //추가 할 것
                //보스전 이전 짧은 UI연출 넣고
                this.StartGameBoss();
            }
            else if (this.stageProgress == 2)
            {
                this.OnStageClear();
            }
        };

        //fadein


        //#패배조건, 시간안에 monster킬수 미달, HP<=0

        //3노래가 끝나면 질주 멈추고 보스 등장, 보스 등장 연출, 보스 전 시작
        //2카운트 다운 끝나면, stage노래와 메트로놈 시작하고 질주 시작
        this.OnStartGame = () =>
        {
            this.StartInput();  //입력 받기 시작
            this.StartGame();
        };
        //1카운트 다운 시작
        StartCoroutine(this.CountDown());

        //적 등장시 질주 멈춤
        //메트로놈에 맞춰서 전투
        //적 사망시 질주 시작

    }

    private void OnTick()
    {
        //비트가 정박일때 아웃라인 반짝이기
        if (this.bitCount % 2 == 0 && this.currentMonster != null)
        {
            StartCoroutine(this.UIOutLinePop());
        }


        #region 비트 카운트가 0~15일때 개별 동작
        if (this.bitCount == 0)
        {
            this.Tick_0();
        }
        else if (this.bitCount == 8)
        {
            this.Tick_8();
        }
        #endregion

        //비트가 0~7일때//몬스터가 살아있으면
        if (this.bitCount < 8 && this.currentMonster != null)
        {
            //패턴 진행
        }

        //비트카운트 업과 초기화
        this.bitCount++;
        if (this.bitCount > 15)
        {
            this.bitCount = 0;
        }
    }

    #region Tick_Num

    private void Tick_0()//몬스터 죽음 판정, 몬스터 없을시 소환, 몬스터 있을 시 패턴 및 대사 On
    {
        if (this.currentMonster != null && this.currentMonster.hp <= 0)
        {
            Debug.Log("몬스터 죽음");
            this.currentMonster = null;
        }

        if (this.currentMonster == null)
        {
            //(bit count==5 에서)몬스터 소환
            this.spawnMonster = true;
        }
        else
        {
            //몬스터가 있을 시 패턴 실행+말풍선 띄우기(4에서 말풍선 지우기)

            //몬스터 패턴 정하고, 대사띄우는 메서드
            this.currentMonster.PartternDice();
        }
    }

    private void Tick_8()
    {
        if (this.spawnMonster)
        {
            this.SpawnMonster();//몬스터 소환
        }
    }
    #endregion

    #region 게임 시작 관련
    private void LoadStagePrefab()
    {
        Debug.Log("LoadPrefab");
        var stagePrefab = Resources.Load<GameObject>(this.stageData.prefabPath);
        this.stagePrefab = Instantiate(stagePrefab).GetComponent<StagePrefab>();
        //스테이지(맵, 오디오, 메트로놈 등 다 stage에서 뽑아쓰기)
        //Ontick을 StagePrefab이 아닌 Stage에 분리시켜서 Stage.cs에서 사용하기
    }//프리팹 로드

    private void StartGame()//게임 시작
    {
        Debug.Log("StartGame");
        this.stagePrefab.StartMusic();    //노래 시작
        this.stagePrefab.MapScrolling();  //맵 스크롤링 시작
        this.hero.Run();    //캐릭터 달리기 애니메이션 시작
    }//게임시작

    private void StartGameBoss()//보스전 시작
    {
        Debug.Log("StartGame");
        this.stagePrefab.StartBossMusic();    //노래 시작
        this.stagePrefab.MapScrolling();  //맵 스크롤링 시작
        this.hero.Run();    //캐릭터 달리기 애니메이션 시작
    }

    private void Encounter()//적 조우
    {
        this.hero.BattleIdle();
        this.stagePrefab.StopMapScrolling();
    }

    private void TimerEnd()
    {
        //노래, 달리기, 메트로놈 중단
        this.stagePrefab.StopMusic();
        this.stagePrefab.StopMapScrolling();
        this.hero.Idle();
    }

    //카운트 다운
    private IEnumerator CountDown()
    {
        this.txt_countDown.gameObject.SetActive(true);
        int i = 5;

        while (true)
        {
            this.txt_countDown.text = i.ToString();
            if (i <= 0)
            {
                this.txt_countDown.gameObject.SetActive(false);
                this.txt_startGame.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                this.txt_startGame.gameObject.SetActive(false);
                this.OnStartGame();
                break;
            }
            i--;
            Debug.Log(i);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator UIOutLinePop()
    {
        var color = this.img_UIOutLine.color;
        float alpha = color.a;

        while (true)
        {
            alpha += 0.34f;
            color.a = alpha;
            this.img_UIOutLine.color = color;

            if (alpha >= 1)
            {
                alpha = 1;
                break;
            }
            yield return null;
        }

        while (true)
        {
            alpha -= 0.16f;
            color.a = alpha;
            this.img_UIOutLine.color = color;

            if (alpha <= 0)
            {
                alpha = 0;
                break;
            }
            yield return null;
        }
    }
    #endregion

    #region 몬스터 소환
    private void Ready2Monster()//몬스터 리스트 준비
    {
        Debug.Log("몬스터 리스트 준비");
        var dic = DataManager.Instance.dicMonsterData;

        int[] monsterID = this.stagePrefab.arrMonsterID;//스테이지프리팹에서 몬스터 아이디들 받아와서
        for (int i = 0; i < monsterID.Length; i++)//전부 생성해서 리스트에 넣기
        {
            var data = dic[monsterID[i]];
            var monsterPrefab = Resources.Load<Monster>(data.prefabPath);
            var monster = Instantiate(monsterPrefab);

            monster.gameObject.SetActive(false);
            this.stageMonsters.Add(monster);
        }
    }

    private void SpawnMonster()
    {
        var dic = DataManager.Instance.dicMonsterData;

        int spawnPercentage = Random.Range(1, 100);
        if (spawnPercentage >= 70)//70%확률로 몬스터 소환
        {
            //몬스터 소환
            int spwanDice = Random.Range(0, this.stageMonsters.Count);
            var monster = this.stageMonsters[spwanDice];

            this.currentMonster = monster;
            this.currentMonster.Spawn();//몬스터의 hp를 초기화하고 오브젝트 등장

            this.spawnMonster = false;
            this.Encounter();//인카운터 : 몬스터 만남을 뜻함
        }
    }
    #endregion

    #region input관련/AB키입력, 터치입력
    private Coroutine inputKeyRoutine;
    private float inputLate = 0.06f;

    private void StartInput()
    {
        StartCoroutine(this.ABAB());    //AD키 입력
        StartCoroutine(this.RayTouch());    //화면 터치
    }

    #region 키보드 AD버전

    private IEnumerator ABAB()
    {
        while (true)
        {
            switch (Input.inputString)
            {
                case "AD":
                case "ad":
                    this.KeyAB();
                    break;
                case "A":
                case "a":
                    if (this.inputKeyRoutine == null)
                    {
                        this.inputKeyRoutine = StartCoroutine(this.InputA());
                    }
                    break;

                case "D":
                case "d":
                    if (this.inputKeyRoutine == null)
                    {
                        this.inputKeyRoutine = StartCoroutine(this.InputB());
                    }
                    break;
            }
            yield return null;
        }
    }

    private IEnumerator InputA()
    {
        float i = this.inputLate;
        bool d = false;
        while (i > 0)
        {
            if (Input.inputString == "D" || Input.inputString == "d")
            {
                d = true;
            }

            i -= Time.deltaTime;
            yield return null;
        }

        if (d == true)
        {
            this.KeyAB();
        }
        else
        {
            this.KeyA();
        }
        this.inputKeyRoutine = null;
    }

    private IEnumerator InputB()
    {
        float i = this.inputLate;
        bool a = false;
        while (i > 0)
        {
            if (Input.inputString == "A" || Input.inputString == "a")
            {
                a = true;
            }

            i -= Time.deltaTime;
            yield return null;
        }

        if (a == true)
        {
            this.KeyAB();
        }
        else
        {
            this.KeyB();
        }
        this.inputKeyRoutine = null;
    }
    #endregion
    #region 터치 입력
    private IEnumerator RayTouch()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Vector2 pos = Input.GetTouch(0).position;    // 터치한 위치
                Vector2 theTouch = new Vector2(pos.x, pos.y);

                Ray ray = Camera.main.ScreenPointToRay(theTouch);    // 터치한 좌표 레이로 바꾸엉
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);    // 정보 저장할 구조체 만들고

                if (hit.collider != null && Input.GetTouch(0).phase == TouchPhase.Began)    // 딱 처음 터치 할때 발생한다
                {
                    if (hit.collider.tag == "KeyA")
                    {
                        if (this.inputKeyRoutine == null)
                        {
                            this.inputKeyRoutine = StartCoroutine(this.TouchA());
                        }
                    }
                    else if (hit.collider.tag == "KeyB")
                    {
                        if (this.inputKeyRoutine == null)
                        {
                            this.inputKeyRoutine = StartCoroutine(this.TouchB());
                        }
                    }
                }
            }
            yield return null;
        }
    }
    private IEnumerator TouchA()
    {
        float i = this.inputLate;
        bool b = false;

        while (i > 0)
        {
            if (Input.touchCount > 1)//터치 개수가 2개 이상일때
            {
                Vector2 pos = Input.GetTouch(1).position;    // 터치한 위치
                Vector2 theTouch = new Vector2(pos.x, pos.y);

                Ray ray = Camera.main.ScreenPointToRay(theTouch);    // 터치한 좌표 레이로 바꾸엉
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);    // 정보 저장할 구조체 만들고

                if (hit.collider.tag == "KeyB")
                {
                    b = true;
                }

            }
            i -= Time.deltaTime;//이중터치 타이머
            yield return null;
        }

        if (b == true)
        {
            this.KeyAB();
        }
        else
        {
            this.KeyA();
        }
        this.inputKeyRoutine = null;
    }

    private IEnumerator TouchB()
    {
        float i = this.inputLate;
        bool a = false;

        while (i > 0)
        {
            if (Input.touchCount > 1)
            {
                Vector2 pos = Input.GetTouch(1).position;    // 터치한 위치
                Vector2 theTouch = new Vector2(pos.x, pos.y);

                Ray ray = Camera.main.ScreenPointToRay(theTouch);    // 터치한 좌표 레이로 바꾸엉
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);    // 정보 저장할 구조체 만들고

                if (hit.collider.tag == "KeyA")
                {
                    a = true;
                }
            }
            i -= Time.deltaTime;//이중터치 타이머
            yield return null;
        }

        if (a == true)
        {
            this.KeyAB();
        }
        else
        {
            this.KeyB();
        }
        this.inputKeyRoutine = null;
    }
    #endregion

    #region 함수 호출부
    private void KeyA()
    {
        Debug.Log("KeyA");
        if (this.perfectZone.Operate)
        {
            this.CheckNote(0);
        }
        else if (this.missZone.Operate)
        {
            //miss
        }
    }

    private void KeyB()
    {
        Debug.Log("KeyB");
        if (this.perfectZone.Operate)
        {
            this.CheckNote(1);
        }
        else if (this.missZone.Operate)
        {
            //miss
        }
    }

    private void KeyAB()
    {
        Debug.Log("KeyAB");
        if (this.perfectZone.Operate)
        {
            this.CheckNote(2);
        }
        else if (this.missZone.Operate)
        {
            //miss
        }
    }

    private void CheckNote(int key)//0 = A, 1 = B, 2 = AB
    {

    }
    #endregion
    #endregion
}
