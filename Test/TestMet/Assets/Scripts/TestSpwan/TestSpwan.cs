using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSpwan : MonoBehaviour
{
    public Button btn_countUp;
    public Button btn_kill;
    public Text txt_count;

    private int[] arrMonsterID;
    private int count = 0;

    private bool existMonster;
    private int monsterHp;
    void Start()
    {
        this.arrMonsterID = new int[] { 1, 2, 3, 4, 5 };

        this.btn_countUp.onClick.AddListener(() =>
        {
            this.Ontick();
        });

        this.btn_kill.onClick.AddListener(() =>
        {
            if (this.existMonster)
            {
                //몬스터 있을때 킬버튼 누름
                Debug.Log("몬스터 죽이기");
                this.monsterHp = 0;
            }
            else
            {
                //몬스터 없을때 킬버튼 누름
                Debug.Log("몬스터가 없습니다.");
            }
        });
    }

    private void Ontick()
    {
        this.txt_count.text = this.count.ToString();

        if (this.count == 0)
        {
            if (this.existMonster)
            {
                //몬스터 있음
                if (this.monsterHp <= 0)
                {
                    Debug.Log("몬스터 죽음");
                    this.existMonster = false;
                }
            }
        }

        if (this.count == 3)
        {
            if (this.existMonster == false)
            {
                this.SpwanMonster();
            }
        }

        this.count++;

        if (this.count > 4)
        {
            Debug.Log("카운트 리셋");
            this.count = 0;
        }
    }

    private void SpwanMonster()
    {
        Debug.Log("몬스터 소환");
        int a = Random.Range(1, 100);
        int b = Random.Range(0, this.arrMonsterID.Length);
        Debug.LogFormat("소환 다이스 : {0}", a);
        if (a > 30)
        {
            Debug.LogFormat("ID : {0}의 몬스터 소환", b);
            this.monsterHp = 10;

            this.existMonster = true;

        }
        else
        {
            Debug.Log("소환하지 않음");
        }
    }
}
