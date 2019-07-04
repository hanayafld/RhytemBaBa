using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBattle : MonoBehaviour
{
    //입력단
    private TestBattleInput input;

    #region CountDown()
    private System.Action OnCountDownEnd;

    public Text txt_CountDown;
    public Text txt_GameStart;
    #endregion

    #region Audio
    private AudioSource bgm_source;

    //Metronome
    public Image img_UIoutLine;

    private float bgm_currentBPM;
    private double startTick;
    private double nextTick = 0.0f;
    private double sampleRate = 0.0f;
    private double spb;

    private bool ticked = false;

    private int bitCount;
    private int audioTimer;

    private Coroutine MetronomeRoutine;
    private Coroutine TimerRoutine;
    private Coroutine RunRoutine;
    #endregion

    #region StagePrefab (AudioSurce, arrMap, bgmData)
    //스테이지 프리팹
    public TestBattleStagePrefab stagePrefab;

    private GameObject[] arrMap;
    private AudioSource bgm_stage;
    public int bgm_stageBPM;
    public int bgm_stageLength;
    public int bgm_stagsStandby;

    private AudioSource bgm_boss;
    public int bgm_bossBPM;
    public int bgm_bossLength;
    public int bgm_bossStandby;
    #endregion

    //히어로
    public TestBattleHero hero;

    #region 전투 관련

    #endregion

    void Start()
    {
        //키 입력 준비
        this.input = this.GetComponent<TestBattleInput>();

        //this.input.OnKeyA = () => { };
        //this.input.OnKeyB = () => { };
        //this.input.OnkeyAB = () => { };

        //프리팹 로드
        this.LoadPrefab();
        //게임 준비
        this.Ready2Stage();

    }

    private void Ready2Stage()
    {
        Debug.Log("Ready2Stage");
        this.audioTimer = this.bgm_stageLength;
        this.bgm_source = this.bgm_stage;
        this.bgm_currentBPM = this.bgm_stageBPM*2;
        Screen.SetResolution(Screen.width, Screen.height, true);
        // 오디오 시스템의 현재 시각을 돌려줍니다.
        //시작틱은, AudioSettings.dspTime; 
        this.startTick = AudioSettings.dspTime;
        //샘플레이트에 믹서의 현재 출력 비율을 가져옵니다.
        this.sampleRate = AudioSettings.outputSampleRate;
        //다음틱은 오디오시스템의 현재 시각 + 60/bpm startTick + (spb)
        this.nextTick = startTick + (60d / this.bgm_currentBPM);
        //오디오의 길이를 데이터 테이블에서 받아서, 오디오 타이머에 넣기
        this.bitCount = -this.bgm_stagsStandby;

        //카운트 다운 한 뒤, 게임 시작
        this.OnCountDownEnd = () =>
        {
            this.StartGame();
        };
        StartCoroutine(this.CountDown());

    }

    private void Ready2Boss()
    {
        Debug.Log("Ready2Boss");
        this.audioTimer = this.bgm_bossLength;
        this.bgm_source = this.bgm_boss;
        this.bgm_currentBPM = this.bgm_bossBPM*2;
        Screen.SetResolution(Screen.width, Screen.height, true);
        // 오디오 시스템의 현재 시각을 돌려줍니다.
        //시작틱은, AudioSettings.dspTime; 
        this.startTick = AudioSettings.dspTime;
        //샘플레이트에 믹서의 현재 출력 비율을 가져옵니다.
        this.sampleRate = AudioSettings.outputSampleRate;
        //다음틱은 오디오시스템의 현재 시각 + 60/bpm startTick + (spb)
        this.nextTick = startTick + (60d / this.bgm_currentBPM);
        //오디오의 길이를 데이터 테이블에서 받아서, 오디오 타이머에 넣기
        this.bitCount = -this.bgm_bossStandby;
    }

    private void StartGame()
    {
        Debug.Log("게임 시작");

        this.StartMusic();

    }

    private IEnumerator CountDown()
    {
        this.txt_CountDown.gameObject.SetActive(true);
        int i = 5;

        while (true)
        {
            this.txt_CountDown.text = i.ToString();
            if (i <= 0)
            {
                this.txt_CountDown.gameObject.SetActive(false);
                this.txt_GameStart.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);
                this.txt_GameStart.gameObject.SetActive(false);
                this.OnCountDownEnd();
                break;
            }
            i--;
            Debug.Log(i);
            yield return new WaitForSeconds(1);
        }
    }

    private void LoadPrefab()
    {
        Debug.Log("Prefab Load");
        this.arrMap = this.stagePrefab.arrMap;
        this.bgm_stage = this.stagePrefab.bgm_stage;
        this.bgm_boss = this.stagePrefab.bgm_boss;
        //몬스터 ID들 받아서 몬스터 데이터 참조하여 몬스터 생성
    }

    #region audio, metronome
    private void StartMusic()
    {
        Debug.Log("달려");
        this.bgm_source.Play();
        this.MetronomeRoutine = StartCoroutine(this.Metronome());
        this.TimerRoutine = StartCoroutine(this.Timer());
        this.hero.anim.Play("run");
        this.RunRoutine = StartCoroutine(this.Run());
    }

    private void PauseMusic()
    {
        this.bgm_source.Pause();
        StopCoroutine(this.MetronomeRoutine);
        StopCoroutine(this.TimerRoutine);
        StopCoroutine(this.RunRoutine);
    }

    private IEnumerator Timer()
    {
        while (this.audioTimer > 0)
        {
            this.audioTimer--;
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Timer끝");
        this.hero.anim.Play("idle");
        StopAllCoroutines();
    }

    private IEnumerator Metronome()
    {
        while (true)
        {
            this.spb = 60.0f / this.bgm_currentBPM;
            //spb = 60/ bpm
            double dspTime = AudioSettings.dspTime;
            //오디오 시스템의 현재 시각

            //오디오 시스템의 현재시각이 Start에서 구한 nextTick보다 크거나 같으면
            while (dspTime >= nextTick)
            {
                ticked = false;
                //bool 틱 = false;
                nextTick += this.spb;
                //다음 틱에 spb를 더해줌
                if (!ticked && nextTick >= AudioSettings.dspTime && this.bgm_source.isPlaying)
                {
                    //bool ticked 를 트루로
                    ticked = true;
                    //온틱 메서드 호출
                    this.OnTick();
                }
            }
            yield return null;
        }
    }

    void OnTick()
    {
        if (this.bitCount % 2 == 0)
        {
            Debug.Log(this.bitCount);//테스트용
            StartCoroutine(this.UIoutLinePop());
        }
        #region 적 스폰 전투 기능 구현
        //소환
        //if(this.bitCount == 0)
            //if(적 HP가 <= 0)
                //bool 적이있음 = false;
                //사망애니메이션과 맵과함께 뒤로 퇴장
            //if(!적이있음)
                //bool 적 소환 = true;
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
    #endregion

    private IEnumerator Run()
    {
        while (true)
        {
            for (int i = 0; i < this.arrMap.Length; i++)
            {
                this.arrMap[i].transform.Translate(new Vector3(-0.1f, 0, 0));
                if (this.arrMap[i].transform.localPosition.x <= -19.2f)
                {
                    this.arrMap[i].transform.localPosition = new Vector3(38.4f, 0, 0);
                }
            }
            yield return null;
        }
    }

    private IEnumerator UIoutLinePop()
    {
        var color = this.img_UIoutLine.color;
        float alpha = color.a;

        while (true)
        {
            alpha += 0.34f;
            color.a = alpha;
            this.img_UIoutLine.color = color;

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
            this.img_UIoutLine.color = color;

            if (alpha <= 0)
            {
                alpha = 0;
                break;
            }
            yield return null;
        }
    }
}
