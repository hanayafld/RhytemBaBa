using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestABAB : MonoBehaviour
{
    private Coroutine inputKeyRoutine;

    void Start()
    {
        StartCoroutine(this.abab());
    }

    private IEnumerator abab()
    {
        while (true)
        {
            switch (Input.inputString)
            {
                case "AD":
                case "ad":
                    Debug.Log("AD");
                    break;
                case "A":
                case "a":
                    if(this.inputKeyRoutine == null)
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
        float i = 0.03f;
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
            Debug.Log("A+D");
        }
        else
        {
            Debug.Log("A");
        }
        this.inputKeyRoutine = null;
    }

    private IEnumerator InputB()
    {
        float i = 0.03f;
        bool a = false;
        while (i > 0)
        {
            if (Input.inputString == "A" || Input.inputString == "a")
            {
                Debug.Log("추가 입력!");
                a = true;
            }

            i-=Time.deltaTime;
            yield return null;
        }

        if (a == true)
        {
            Debug.Log("D+A");
        }
        else
        {
            Debug.Log("D");
        }
        this.inputKeyRoutine = null;
    }

}
