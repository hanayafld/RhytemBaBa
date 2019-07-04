using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public System.Action OnStageClear;
    public System.Action OnStageRestart;
    public System.Action OnStageFailed;

    private HeroInfo heroinfo;
    private StageData stageData;

    public Image img_dim;

    private Hero hero;
    
    public void Init(HeroInfo heroInfo)
    {
        Debug.Log("Stage Init");
        //인포 불러오기
        this.heroinfo = heroInfo;

        //데이터 불러오기(스테이지 데이터와 몬스터 데이터)
        DataManager.Instance.LoadAllData();
        this.stageData = DataManager.Instance.dicStageData[this.heroinfo.stageLevel];

        //fadein

        //게임 준비완료

    }

    private void Ready2Game()
    {
        //#패배조건, 시간안에 monster킬수 미달, HP0

        //3노래가 끝나면 질주 멈추고 보스 등장, 보스 등장 연출, 보스 전 시작
        //2카운트 다운 끝나면, stage노래와 메트로놈 시작하고 질주 시작
        //1카운트 다운 시작


        //적 등장시 질주 멈춤
        //메트로놈에 맞춰서 전투
        //적 사망시 질주 시작


    }

    private void LoadPrefab()
    {
        var stagePrefab = Resources.Load<GameObject>(this.stageData.prefabPath);
        var stage = Instantiate(stagePrefab).GetComponent<StagePrefab>();

        //this.hero = 
    }

}
