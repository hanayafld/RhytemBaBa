using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public System.Action OnDiedHero;

    private Animator anim;
    private Dictionary<int, HeroData> dicHeroData;
    void Awake()
    {
        this.anim = GetComponentInChildren<Animator>();

        DataManager.Instance.LoadAllData();
        dicHeroData = DataManager.Instance.dicHeroData;
    }

    public void Run()
    {
        this.anim.Play("run");
    }

    public void Idle()
    {
        this.anim.Play("idle");
    }
}
