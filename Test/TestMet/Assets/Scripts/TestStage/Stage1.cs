using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Stage1 : MonoBehaviour
{
    public Button btn_StartGame;
    public Button btn_Option;
    private bool option;

    public Text txt_CountDown;
    public Text txt_GameStart;

    private Dictionary<int, StageData> dicStageData;

    private GameObject stage01;
    private int audioLength;
    private GameObject[] arrMaps;

    private System.Action OnStartGame;

    #region Audio
    private AudioSource bg_Music;

    //Metronome
    private double bpm;
    private double startTick;
    private double nextTick = 0.0f;
    private double sampleRate = 0.0f;

    private double spb;
    private bool ticked = false;

    private int bitCount = 0;
    private int audioTimer;
    #endregion

    private Coroutine MetronomeRoutine;
    private Coroutine TimerRoutine;
    private Coroutine RunRoutine;

    void Awake()
    {
        dicStageData = new Dictionary<int, StageData>();
    }

    void Start()
    {
        Debug.Log("Stage Init");
        //1.데이터 로드
        this.LoadData();
        //2.프리팹 로드
        this.LoadStage();

        this.OnStartGame = () =>
          {
              this.StartGame();
          };

        this.btn_StartGame.onClick.AddListener(() =>
        {
            this.Ready2Game();

            StartCoroutine(this.CountDown());
        });

        this.btn_Option.onClick.AddListener(() =>
        {
            if (this.option)
            {
                //일시정지 풀기
                StartCoroutine(this.CountDown());
                this.option = false;

            }
            else
            {
                //일시정지
                this.PauseMusic();
                this.option = true;
            }
        });
    }

    private void Ready2Game()
    {
        Screen.SetResolution(Screen.width, Screen.height, true);
        // 오디오 시스템의 현재 시각을 돌려줍니다.
        //시작틱은, AudioSettings.dspTime; 
        this.startTick = AudioSettings.dspTime;
        //샘플레이트에 믹서의 현재 출력 비율을 가져옵니다.
        this.sampleRate = AudioSettings.outputSampleRate;
        //다음틱은 오디오시스템의 현재 시각 + 60/bpm startTick + (spb)
        this.nextTick = startTick + (60d / bpm);
        //오디오의 길이를 데이터 테이블에서 받아서, 오디오 타이머에 넣기
        this.bitCount = 0;
        this.audioTimer = this.audioLength;
    }

    private void StartGame()
    {
        Debug.Log("게임 시작");

        this.StartMusic();

    }

    //1.데이터 로드
    private void LoadData()
    {
        var json = Resources.Load<TextAsset>("Data/StageData");
        var arrStageData = JsonConvert.DeserializeObject<StageData[]>(json.text);
        foreach (var data in arrStageData)
        {
            this.dicStageData.Add(data.id, data);
        }
    }

    //2.프리팹 로드
    private void LoadStage()
    {
        var stageGO = Resources.Load<GameObject>(this.dicStageData[1].stage_PreFabPath);
        this.bpm = this.dicStageData[1].audio_BPM;
        this.audioLength = this.dicStageData[1].audio_Length;

        this.stage01 = Instantiate(stageGO);
        this.stage01.gameObject.name = "Stage01";
        var data = this.stage01.GetComponent<StagePrefab>();
        this.bg_Music = data.bg_Music;
        this.arrMaps = data.arrMaps;

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
                this.OnStartGame();
                yield return new WaitForSeconds(1);
                this.txt_GameStart.gameObject.SetActive(false);

                break;
            }
            i--;
            Debug.Log(i);
            yield return new WaitForSeconds(1);
        }
    }

    #region audio, metronome
    private void StartMusic()
    {
        this.bg_Music.Play();
        this.MetronomeRoutine = StartCoroutine(this.Metronome());
        this.TimerRoutine = StartCoroutine(this.Timer());
        this.RunRoutine = StartCoroutine(this.Run());
    }

    private void PauseMusic()
    {
        this.bg_Music.Pause();
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
        StopAllCoroutines();
    }

    private IEnumerator Metronome()
    {
        while (true)
        {
            this.spb = 60.0f / bpm;
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
                if (!ticked && nextTick >= AudioSettings.dspTime && this.bg_Music.isPlaying)
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
            Debug.LogFormat("<color=red>{0}</color>", this.bitCount);
        }
        else
        {
            Debug.LogFormat("<color=blue>{0}</color>", this.bitCount);
        }
        this.bitCount++;
    }
    #endregion

    private IEnumerator Run()
    {
        while (true)
        {
            for (int i = 0; i < this.arrMaps.Length; i++)
            {
                this.arrMaps[i].transform.Translate(new Vector3(-0.1f, 0, 0));
                if (this.arrMaps[i].transform.localPosition.x <= -19.2f)
                {
                    this.arrMaps[i].transform.localPosition = new Vector3(38.4f, 0, 0);
                }
            }
            yield return null;
        }
    }
}
