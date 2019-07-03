using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTouch : MonoBehaviour
{
    public Text txt_text;
    public Text txt_touch0;
    public Text txt_touch1;
    public Button btn_option;
    public Image img_dim;

    private float inputLate = 0.03f;

    private Coroutine inputKeyRoutine;

    void Start()
    {
        this.btn_option.onClick.AddListener(() =>
        {
            this.txt_text.text = "옵션";
            if (this.img_dim.gameObject.activeSelf)
            {
                this.img_dim.gameObject.SetActive(false);
            }
            else
            {
                this.img_dim.gameObject.SetActive(true);
            }
        });

        StartCoroutine(this.RayTouch());

    }

    private IEnumerator RayTouch()
    {
        Debug.Log("RayTouch");

        while (true)
        {
            if (Input.touchCount > 0)
            {
                Vector2 pos = Input.GetTouch(0).position;    // 터치한 위치
                Vector2 theTouch = new Vector2(pos.x, pos.y);

                this.txt_touch0.text = string.Format("({0}, {1})", (int)pos.x, (int)pos.y);

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
                    else
                    {
                        Debug.Log("뷁");
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

                this.txt_touch1.text = string.Format("({0}, {1})", (int)pos.x, (int)pos.y);

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

        while (i > 0)
        {
            if (Input.touchCount > 1)
            {
                Vector2 pos = Input.GetTouch(1).position;    // 터치한 위치
                Vector2 theTouch = new Vector2(pos.x, pos.y);

                this.txt_touch1.text = string.Format("({0}, {1})", (int)pos.x, (int)pos.y);

                Ray ray = Camera.main.ScreenPointToRay(theTouch);    // 터치한 좌표 레이로 바꾸엉
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);    // 정보 저장할 구조체 만들고

                if (hit.collider.tag == "KeyA")
                {
                    Debug.Log("추가 입력!");
                    a = true;
                }

            }
            i -= Time.deltaTime;//이중터치 타이머
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
