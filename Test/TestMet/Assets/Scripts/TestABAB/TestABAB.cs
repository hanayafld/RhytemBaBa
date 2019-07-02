using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestABAB : MonoBehaviour
{
    private Coroutine inputKeyRoutine;
    private float inputLate = 0.03f;

    public Text txt_text;

    void Start()
    {
        StartCoroutine(this.ABAB());//키보드 AD버전
        StartCoroutine(this.Touch());//터치버전
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
                    Debug.Log("AD");
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
                Debug.Log("추가 입력!");
                d = true;
            }

            i -= Time.deltaTime;
            yield return null;
        }

        if (d == true)
        {
            Debug.Log("A+B");
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            Debug.Log("A");
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
            Debug.Log("B+A");
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            Debug.Log("B");
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

        while (i>0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider.tag == "KeyB")
                {
                    Debug.Log("추가 입력!");
                    b = true;
                }

                i -= Time.deltaTime;
            }
            yield return null;
        }

        if (b == true)
        {
            Debug.Log("A+B");
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            Debug.Log("A");
            //a입력 메서드
            this.KeyA();
        }
        this.inputKeyRoutine = null;
    }

    private IEnumerator TouchB()
    {
        float i = this.inputLate;
        bool a = false;

        while (i>0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider.tag == "KeyA")
                {
                    Debug.Log("추가 입력!");
                    a = true;
                }

                i -= Time.deltaTime;
            }
            yield return null;
        }

        if (a == true)
        {
            Debug.Log("B+A");
            //ad동시입력 메서드
            this.KeyAB();
        }
        else
        {
            Debug.Log("B");
            //a입력 메서드
            this.KeyB();
        }
        this.inputKeyRoutine = null;
    }
    #endregion

    private void KeyA()
    {
        this.txt_text.text = "A";
    }

    private void KeyB()
    {
        this.txt_text.text = "B";
    }

    private void KeyAB()
    {
        this.txt_text.text = "AB";
        Handheld.Vibrate();
    }
}
