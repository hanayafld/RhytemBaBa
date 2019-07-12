using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNoteNote : MonoBehaviour
{
    public System.Action OnOutNote;

    private float speed = 130 / 900f;
    [HideInInspector]
    public int type;

    public void Init(int type)
    {

        this.type = type;

        if (this.type != 0)
        {
            this.gameObject.SetActive(true);
            StartCoroutine(this.MoveNote());
        }

        switch (type)
        {
            case 0:
                //노트생성 안 함
                break;
            case 1:
                //일반 노트
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 2:
                //강한 노트
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                break;
            case 4:
                //방어노트
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 5:
                //휴식 노트
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }


    }


    private IEnumerator MoveNote()
    {
        while (true)
        {
            if (this.transform.position.x <= -7)
            {
                //아웃 판정 메서드(노트의 타입에따라 데미지 or 효과)
                this.OutNote();
            }

            this.transform.Translate(new Vector3(-this.speed, 0, 0));

            yield return null;
        }
    }

    private void OutNote()
    {
        this.gameObject.SetActive(false);
        this.transform.localPosition = Vector3.zero;

        //this.OnOutNote();

        StopAllCoroutines();
    }
}
