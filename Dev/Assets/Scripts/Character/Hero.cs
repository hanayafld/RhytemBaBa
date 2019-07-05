using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public System.Action OnDiedHero;

    [HideInInspector]
    private Animator anim;

    void Awake()
    {
        this.anim = GetComponentInChildren<Animator>();

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
