using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trader : MonoBehaviour
{
    private System.Action OnComeEnd;

    [HideInInspector]
    public Animator anim;
    public GameObject speechBubble;

    private bool clickCheck;

    void Start()
    {
        this.anim = GetComponentInChildren<Animator>();

        this.OnComeEnd = () =>
        {
            //대사 활성화
            StartCoroutine(this.Speech());
            //상점 활성화
            
        };

        StartCoroutine(this.Come());
    }

    //NPC 접근
    private IEnumerator Come()
    {
        //나중에 추가
        //this.anim.Play(걷기);

        while (true)
        {
            if (this.transform.position.x < 2)
            {
                Debug.Log("이동 완료");
                //this.anim.Play(앉기);
                this.OnComeEnd();
                break;
            }
            this.transform.Translate(-0.02f, 0, 0);

            yield return null;
        }
    }

    //NPC대사
    private IEnumerator Speech()
    {
        yield return new WaitForSeconds(2);

        while(true)
        {
            if(this.clickCheck)
            {
                Debug.Log("대사 그만");
                break;
            }

            this.speechBubble.SetActive(true);

            yield return new WaitForSeconds(2);

            this.speechBubble.SetActive(false);

            yield return new WaitForSeconds(4);
            
            yield return null;
        }
    }
}
