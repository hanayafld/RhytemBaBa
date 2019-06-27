using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    
    //Resources
    public AudioSource bgMusic;

    //Metronome
    public double bpm;
    public double startTick;
    public double nextTick = 0.0f;
    public double sampleRate = 0.0f;

    private double spb;
    private bool ticked = false;

    private int bitCount = 0;

    //음악 껏다 켯다 하는 용
    public Button btn_Start;
    private bool musicOnOff;
    private Coroutine r1;
    private Coroutine r2;

    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.height, true);
        //this.img_OutLine.transform.localScale= new Vector3(Screen.width, Screen.height,1);
        this.startTick = AudioSettings.dspTime;
        //시작틱은, AudioSettings.dspTime; 
        // 오디오 시스템의 현재 시각을 돌려줍니다.
        this.sampleRate = AudioSettings.outputSampleRate;
        //샘플레이트에 믹서의 현재 출력 비율을 가져옵니다.
        this.nextTick = startTick + (60d / bpm);
        //다음틱은 오디오시스템의 현재 시각 + 60/bpm startTick + (spb)

        this.btn_Start.onClick.AddListener(() =>
        {
            if (!this.musicOnOff)
            {
                //음악 시작
                this.bgMusic.gameObject.SetActive(true);
                this.r1 = StartCoroutine(this.Metronome());
                //this.r2 = StartCoroutine(this.Up());
                this.musicOnOff = true;
            }
            else
            {
                this.bgMusic.gameObject.SetActive(false);
                StopCoroutine(r1);
                //StopCoroutine(r2);
                this.musicOnOff = false;
                this.bitCount = 0;
            }
        });
    }
    
    //메트로놈 설정
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
                if (!ticked && nextTick >= AudioSettings.dspTime && this.bgMusic.isPlaying)
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
}
