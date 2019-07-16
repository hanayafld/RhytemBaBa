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

    void Awake()
    {
        this.anim = GetComponentInChildren<Animator>();
    }

    public void Spawn()
    {
        DataManager.Instance.LoadAllData();
        var data = DataManager.Instance.dicMonsterData[this.id];
        this.id = data.id;
        this.hp = data.hp;
        this.damage = data.damage;
    }
}
