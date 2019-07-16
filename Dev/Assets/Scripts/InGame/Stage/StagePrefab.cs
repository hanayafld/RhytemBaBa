using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePrefab : MonoBehaviour
{

    //데이터
    private StageData stageData;

    #region 맵 관련
    public GameObject[] arrMaps;
    public Coroutine mapScrollRoutine;
    public float speed = 1;
    private float defaultSpeed = -0.1f;
    #endregion

    #region 오디오, 메트로놈 관련
    public System.Action OnTimerEnd;//타이머 끝나면 호출(Stage.cs에서 받음)
    public System.Action OnTick;//메트로놈 깜빡일때마다 호출(Stage.cs에서 받음)

    private Coroutine timerRoutine;
    private Coroutine metronomeRoutine;

    public AudioSource bgm_source;
    public AudioSource bgm_bossSource;

    private AudioSource bgm_currentSource;
    private float bgm_currentBPM;
    private int bgm_currentLength;

    private double startTick;
    private double nextTick = 0.0f;
    private double sampleRate = 0.0f;
    private double spb;

    private bool ticked = false;

    private int bitCount;
    private int audioTimer;
    #endregion

    #region 몬스터 관련
    public int[] arrMonsterID;
    #endregion

    void Awake()
    {
        //인포 불러오기 why? StageLevel가져오려고
        InfoManager.Instance.LoadInfo();
        var stageLevel = InfoManager.Instance.heroInfo.stageLevel;
        DataManager.Instance.LoadAllData();
        this.stageData = DataManager.Instance.dicStageData[stageLevel];

    }

    #region 맵 관련
    public void MapScrolling()
    {
        if (this.mapScrollRoutine != null)
        {
            this.mapScrollRoutine = null;
        }
        this.mapScrollRoutine = StartCoroutine(this.MapScrollingImpl());
    }

    private IEnumerator MapScrollingImpl()
    {
        while (true)
        {
            for (int i = 0; i < this.arrMaps.Length; i++)
            {
                this.arrMaps[i].transform.Translate(new Vector3(this.defaultSpeed * this.speed, 0, 0));
                if (this.arrMaps[i].transform.localPosition.x <= -19.2f)
                {
                    this.arrMaps[i].transform.localPosition = new Vector3(38.4f, 0, 0);
                }
            }
            yield return null;
        }
    }

    public void StopMapScrolling()
    {
        this.StopCoroutine(this.mapScrollRoutine);
        //this.mapScrollRoutine = null;
    }
    #endregion

    #region 오디오, 메트로놈 관련
    public void StartMusic()
    {
        Debug.Log("스테이지 오디오 셋팅");
        this.bgm_currentSource = this.bgm_source;   //기본 음악 소스
        this.audioTimer = this.stageData.bgm_Length;    //기본 음악 길이
        this.bgm_currentBPM = this.stageData.bgm_BPM * 2;    //기본 음악 BPM(반박자 표현을 위해서 *2)
        this.bitCount = -this.stageData.bgm_Standby;
        this.SetMetronome();
    }

    public void StartBossMusic()
    {
        Debug.Log("보스전 오디오 셋팅");
        this.bgm_currentSource = this.bgm_bossSource;   //보스 음악 소스
        this.audioTimer = this.stageData.bgm_bossLength;    //보스 음악 길이
        this.bgm_currentBPM = this.stageData.bgm_BPM * 2;   //보스 음악 BPM(반박자 표현을 위해서 *2)
        this.bitCount = -this.stageData.bgm_bossStandby;
        this.SetMetronome();
    }

    public void StopMusic()
    {
        StopCoroutine(this.timerRoutine);
        StopCoroutine(this.metronomeRoutine);
        this.timerRoutine = null;
        this.metronomeRoutine = null;
    }

    private void SetMetronome()
    {
        //메트로놈 준비 항목
        this.startTick = AudioSettings.dspTime;
        this.sampleRate = AudioSettings.outputSampleRate;
        this.nextTick = startTick + (60d / this.bgm_currentBPM);

        this.bgm_currentSource.Play();
        this.timerRoutine = StartCoroutine(this.Timer());
        this.metronomeRoutine = StartCoroutine(this.Metronome());
    }

    private IEnumerator Timer()
    {
        while (this.audioTimer > 0)
        {
            this.audioTimer--;
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Timer끝");
        this.OnTimerEnd();
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




    #endregion

}
