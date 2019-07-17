using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public System.Action OnDieMonster;

    public int id;

    [HideInInspector]
    public Animator anim;
    public int damage;
    public int hp;
    public List<int> currentParttern;

    private List<String> partterns;//문자열 인트를 담아두는 곳

    void Awake()
    {
        DataManager.Instance.LoadAllData();
        var data = DataManager.Instance.dicMonsterData[this.id];

        this.anim = GetComponentInChildren<Animator>();
        this.currentParttern = new List<int>();
        this.partterns = new List<String>();
        this.partterns.Add(data.pattern_00);
        this.partterns.Add(data.pattern_01);
        this.partterns.Add(data.pattern_02);
        this.partterns.Add(data.pattern_03);
    }

    public void Spawn()
    {
        DataManager.Instance.LoadAllData();
        var data = DataManager.Instance.dicMonsterData[this.id];
        this.id = data.id;
        this.hp = data.hp;
        this.damage = data.damage;
        this.gameObject.SetActive(true);
    }

    public void PartternDice()
    {
        var partternID = UnityEngine.Random.Range(0, this.partterns.Count);
        var aa = this.partterns[partternID];//한줄짜리 패턴
        for (int i = 0; i < aa.Length; i += 2)
        {
            var aaa = string.Format(aa[i].ToString() + aa[i + 1].ToString());//i와 i+1을 붙임//00, 01과 같은 두자릿수 패턴1개
            this.currentParttern.Add(Int32.Parse(aaa));//int 형으로 변환하여, 현재의 패턴에 보관
        }
        //this.ShowSpeechBubble();//대사 띄우는 메서드 만들기 코루틴으로 일정 시간 있다가 대사지우는 것도 한꺼번에 할까요?
        //아니면 다음에 대사 끄기 허출하면 끌까요?
    }
}
