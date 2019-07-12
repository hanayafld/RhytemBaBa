using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestNote : MonoBehaviour
{
    //Test
    public Text txt_combo;
    private int combo = 0;

    private Coroutine inputKeyRoutine;
    private float inputLate = 0.1f;

    #region Audio
    public AudioSource bgm_source;

    //Metronome
    public Image img_UIoutLine;

    public float bpm;
    private float bgm_currentBPM;
    private double startTick;
    private double nextTick = 0.0f;
    private double sampleRate = 0.0f;
    private double spb;

    private bool ticked = false;

    private int bitCount;
    public int audioTimer;

    private Coroutine MetronomeRoutine;
    private Coroutine TimerRoutine;
    private Coroutine RunRoutine;
    #endregion

    private int noteCount;

    public float speed;
    public TestNoteGoal goal;
    public TestNoteNote[] note;

    public Button btn_Start;

    void Start()
    {
        this.bgm_currentBPM = bpm * 2;
        StartCoroutine(this.ABAB());//키보드 AD버전
        StartCoroutine(this.Touch());//터치버전

        var outNote = this.goal.outNote;
        this.btn_Start.onClick.AddListener(() =>
        {
            Debug.Log("테스트 시작");
            this.StartMusic();
        });
    }


    #region audio, metronome
    private void StartMusic()
    {
        Debug.Log("달려");
        this.bgm_source.Play();
        this.MetronomeRoutine = StartCoroutine(this.Metronome());
        this.TimerRoutine = StartCoroutine(this.Timer());
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
        var timer = this.audioTimer;
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Timer끝");
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
        if (this.bitCount < 8)
        {
            Debug.Log(this.bitCount);//테스트용
            this.CreateNote();
            StartCoroutine(this.UIoutLinePop());
        }
        #region 적 스폰 전투 기능 구현



        #endregion
        this.bitCount++;
        if (this.bitCount > 15)
        {
            this.bitCount = 0;
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
    #endregion

    private void CreateNote()
    {
        int[] aaa = { 1, 0, 1, 0, 1, 0, 1, 0 };//모의 패턴

        if (this.noteCount >= 16)
        {
            this.noteCount = 0;
        }

        this.note[noteCount].Init(aaa[this.bitCount]);
        this.noteCount++;
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
                    //바로 ad동시입력 메서드
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
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            //a입력 메서드
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
                Debug.Log("추가 입력!");
                a = true;
            }

            i -= Time.deltaTime;
            yield return null;
        }

        if (a == true)
        {
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            //d입력 메서드
            this.KeyB();
        }
        this.inputKeyRoutine = null;
    }
    #endregion

    #region 터치버전
    private IEnumerator Touch()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "KeyA" && hit.collider.tag == "KeyB")
                    {
                        this.KeyAB();
                    }
                    else if (hit.collider.tag == "KeyA")
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
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider.tag == "KeyB")
                {
                    b = true;
                }

                i -= Time.deltaTime;
            }
            yield return null;
        }

        if (b == true)
        {
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            //a입력 메서드
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
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider.tag == "KeyA")
                {
                    a = true;
                }

                i -= Time.deltaTime;
            }
            yield return null;
        }

        if (a == true)
        {
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            //a입력 메서드
            this.KeyB();
        }
        this.inputKeyRoutine = null;
    }
    #endregion

    private void KeyA()
    {
        if (this.goal.Operate)
        {
            this.CheckNote(0);
        }
    }

    private void KeyB()
    {
        if (this.goal.Operate)
        {
            this.CheckNote(1);
        }
    }

    private void KeyAB()
    {
        if (this.goal.Operate)
        {
            this.CheckNote(2);
        }
    }

    private void CheckNote(int ab)
    {
        switch (this.goal.currentNote.type)
        {
            case 1:
                //공격노트
                if (ab == 0)
                {
                    //A를 눌렀을때
                    this.combo++;
                    this.txt_combo.text = this.combo.ToString();
                }
                else if (ab == 1)
                {
                    //B를 눌렀을때
                    if (this.combo > 1)
                    {
                        this.combo--;
                    }
                    this.txt_combo.text = this.combo.ToString();
                }
                else if (ab == 2)
                {
                    //AB를 눌렀을때
                    this.combo = 0;
                    this.txt_combo.text = this.combo.ToString();
                }
                break;
            case 2:
                //강공격노트
                if (ab == 0)
                {
                    //A를 눌렀을때
                    this.combo = 0;
                    this.txt_combo.text = this.combo.ToString();
                }
                else if (ab == 1)
                {
                    //B를 눌렀을때
                    this.combo++;
                    this.txt_combo.text = this.combo.ToString();
                }
                else if (ab == 2)
                {
                    //AB를 눌렀을때
                    if (this.combo > 1)
                    {
                        this.combo--;
                    }
                    this.txt_combo.text = this.combo.ToString();
                }
                break;
            case 4:
                //방어노트
                if (ab == 0)
                {
                    //A를 눌렀을때

                }
                else if (ab == 1)
                {
                    //B를 눌렀을때
                }
                else if (ab == 2)
                {
                    //AB를 눌렀을때
                    this.combo++;
                    this.txt_combo.text = this.combo.ToString();
                }
                break;
            case 5:
                //휴식노트
                if (ab == 0)
                {
                    //A를 눌렀을때

                }
                else if (ab == 1)
                {
                    //B를 눌렀을때
                    this.combo++;
                    this.txt_combo.text = this.combo.ToString();
                }
                else if (ab == 2)
                {
                    //AB를 눌렀을때
                    this.combo++;
                    this.combo++;
                    this.txt_combo.text = this.combo.ToString();
                }
                break;
        }
        this.goal.currentNote.gameObject.SetActive(false);
        this.goal.currentNote.transform.localPosition = Vector3.zero;
    }
    //private void OutNote(int type)
    //{
    //    switch (type)
    //    {
    //        case 1:
    //            this.combo = 0;
    //            this.txt_combo.text = this.combo.ToString();
    //            break;
    //        case 2:
    //            this.combo = 0;
    //            this.txt_combo.text = this.combo.ToString();
    //            break;
    //        case 4:
    //            if(this.combo>1)
    //            {
    //                this.combo--;
    //            }
    //            this.txt_combo.text = this.combo.ToString();
    //            break;
    //        case 5:
    //            break;

    //    }

    //}

}
