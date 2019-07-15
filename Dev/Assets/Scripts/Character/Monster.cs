using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public System.Action OnDieMonster;

    public int id;

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public int hp;

    void Awake()
    {
        this.anim = GetComponentInChildren<Animator>();
        DataManager.Instance.LoadAllData();

    }
}
