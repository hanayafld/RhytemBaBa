using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public System.Action OnOutNote;

    private float speed;
    [HideInInspector]
    public int type;

    public void CreateNote(float bpm, int type)
    {

        this.speed = bpm / 900f;
        this.type = type;

        if (this.type != 0)
        {
            this.gameObject.SetActive(true);
            StartCoroutine(this.MoveNote());
        }

        #region 나중에 노트의 모양 변경할때 손볼 것
        /*switch (type)
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
        }*/
        #endregion
    }

    private IEnumerator MoveNote()
    {
        while (true)
        {
            if (this.transform.position.x <= -9)
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

        //this.OnOutNote();//나중에 아웃에 따른 패널티 만들때 호출

        StopAllCoroutines();
    }
}
