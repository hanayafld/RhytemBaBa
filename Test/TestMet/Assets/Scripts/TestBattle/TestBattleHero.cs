using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleHero : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;

    void Start()
    {
        this.anim = GetComponentInChildren<Animator>();

    }
}
