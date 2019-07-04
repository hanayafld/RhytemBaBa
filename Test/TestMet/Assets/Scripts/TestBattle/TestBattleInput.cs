using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleInput : MonoBehaviour
{
    public System.Action OnKeyA;
    public System.Action OnKeyB;
    public System.Action OnkeyAB;

    private Coroutine inputKeyRoutine;
    private float inputLate = 0.07f;

    void Start()
    {
        StartCoroutine(this.KeyABAB());
        StartCoroutine(this.TouchABAB());
    }

    #region 키보드 AD버전
    private IEnumerator KeyABAB()
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

    #region 터치 AB버전
    private IEnumerator TouchABAB()
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
        Debug.Log("키 입력 A");
        //this.OnKeyA();
    }

    private void KeyB()
    {
        Debug.Log("키 입력 B");
        //this.OnKeyB();
    }

    private void KeyAB()
    {
        Debug.Log("키 입력 AB");
        //this.OnkeyAB();
        Handheld.Vibrate();//휴대폰 진동(나중에 옵션인포 추가해서 진동 켜고 끄고 할 수 있게하기)
    }
}
