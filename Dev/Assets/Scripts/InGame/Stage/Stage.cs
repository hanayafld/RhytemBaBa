using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public System.Action OnStageClear;
    public System.Action OnStageRestart;
    public System.Action OnStageFailed;

    private HeroInfo heroinfo;
    private StageData stageData;

    public Image img_dim;

    public Hero hero;
    private Monster currentMonster;
    private StagePrefab stage;

    #region Ontick용(게임진행용)
    private int bitCount;
    private int stageProgress;//0:스테이지 시작 1:스테이지 클리어 2:보스클리어
    private bool isEnemy;

    #endregion

    public void Init(HeroInfo heroInfo)
    {
        Debug.Log("Stage Init");
        //인포 불러오기
        this.heroinfo = heroInfo;

        //데이터 불러오기(스테이지 데이터와 몬스터 데이터)
        DataManager.Instance.LoadAllData();
        this.stageData = DataManager.Instance.dicStageData[this.heroinfo.stageLevel];

        //스테이지 프리팹 불러오기
        this.LoadPrefab();

        this.stage.OnTick = () =>
        {
            this.OnTick();
        };
        this.stage.OnTimerEnd = () =>
        {
            this.TimerEnd();
            this.stageProgress++;
            if (this.stageProgress == 1)
            {
                //추가 할 것
                //보스전 이전 짧은 UI연출 넣고
                this.StartGameBoss();
            }
            else if(this.stageProgress==2)
            {
                this.OnStageClear();
            }
        };

        //fadein


        //#패배조건, 시간안에 monster킬수 미달, HP<=0

        //3노래가 끝나면 질주 멈추고 보스 등장, 보스 등장 연출, 보스 전 시작
        //2카운트 다운 끝나면, stage노래와 메트로놈 시작하고 질주 시작
        this.StartInput();  //입력 받기 시작
        this.StartGame();
        //1카운트 다운 시작


        //적 등장시 질주 멈춤
        //메트로놈에 맞춰서 전투
        //적 사망시 질주 시작

    }

    private void LoadPrefab()
    {
        Debug.Log("LoadPrefab");
        var stagePrefab = Resources.Load<GameObject>(this.stageData.prefabPath);
        this.stage = Instantiate(stagePrefab).GetComponent<StagePrefab>();
        //스테이지(맵, 오디오, 메트로놈 등 다 stage에서 뽑아쓰기)
        //Ontick을 StagePrefab이 아닌 Stage에 분리시켜서 Stage.cs에서 사용하기
    }//프리팹 로드

    private void StartGame()
    {
        Debug.Log("StartGame");
        this.stage.StartMusic();    //노래 시작
        this.stage.MapScrolling();  //맵 스크롤링 시작
        this.hero.Run();    //캐릭터 달리기 애니메이션 시작
    }//게임시작

    private void StartGameBoss()
    {
        Debug.Log("StartGame");
        this.stage.StartBossMusic();    //노래 시작
        this.stage.MapScrolling();  //맵 스크롤링 시작
        this.hero.Run();    //캐릭터 달리기 애니메이션 시작
    }

    private void TimerEnd()
    {
        //노래, 달리기, 메트로놈 중단
        this.stage.StopMusic();
        this.stage.StopMapScrolling();
        this.hero.Idle();
    }

    private void OnTick()
    {
        if (this.bitCount % 2 == 0)
        {
            Debug.Log(this.bitCount);//테스트용
        }
        #region 적 스폰 전투 기능 구현
        //소환
        //if(this.bitCount == 0)
        //if(적 HP가 <= 0)
        //bool 적이있음 = false;
        //사망애니메이션과 맵과함께 뒤로 퇴장
        //if(적이있음 == true)
        //

        //else
        //Random패턴 = 말풍선 띄우기
        //if(this.bitCount == 8)
        //if(적이있음)
        //말풍선 지우기
        //else
        //적 소환하기 Random



        #endregion
        this.bitCount++;
        if (this.bitCount > 15)
        {
            this.bitCount = 0;
        }
    }


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
                    Debug.Log("추가 입력!");
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
    }

    private void KeyB()
    {
        Debug.Log("KeyB");
    }

    private void KeyAB()
    {
        Debug.Log("KeyAB");
        //Handheld.Vibrate();
    }
    #endregion
    #endregion
}
