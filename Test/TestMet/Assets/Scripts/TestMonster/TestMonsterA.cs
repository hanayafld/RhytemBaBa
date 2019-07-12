using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterA : TestRawMonster, IAttack
{
    void Awake()
    {
        Debug.Log("선재공격");
    }

    public override void Run()
    {
        Debug.Log("MonsterA Run");
    }

    public override void Attack()
    {
        Debug.Log("MonsterA Attack");
    }
}
