using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour
{
    //몬스터 테스트
    public TestRawMonster monster;
    //몬스터를 상속받은 몬스터A의 기능을 사용해보자

    void Start()
    {
        this.monster.Run();
        this.monster.Attack();
        
    }
    
}