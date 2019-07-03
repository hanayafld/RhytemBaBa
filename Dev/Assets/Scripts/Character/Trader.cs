using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trader : MonoBehaviour
{
    public System.Action OnComeEnd;

    [HideInInspector]
    public Animator anim;
    public Button btn_trade;
    public GameObject speechBubble;

    void Start()
    {
        this.anim = GetComponentInChildren<Animator>();
        
    }

    public IEnumerator Come()
    {
        //this.anim.Play(걷기);
        while(true)
        {
            if (this.transform.position.x < 2)
            {
                Debug.Log("이동 완료");
                //this.anim.Play(앉기);
                this.OnComeEnd();
                break;
            }

            this.transform.Translate(-0.03f, 0, 0);
            yield return null;
        }
    }

}
