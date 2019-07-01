using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    public System.Action OnLogoSceneEnd;

    public void Init()
    {
        Debug.Log("Logo Init");
        //로고 페이드 인
        //페이드 아웃
        //앱으로 로고 씬 종료를 보냄
        this.OnLogoSceneEnd();
    }
}
