using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public System.Action OnDieMonster;

    internal Animator anim;
    
    void Awake()
    {
        this.anim = GetComponentInChildren<Animator>();
    }

}
