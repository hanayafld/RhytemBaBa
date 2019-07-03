using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public System.Action OnStageClear;
    public System.Action OnStageRestart;
    public System.Action OnStageFailed;

    private HeroInfo heroinfo;

    public void Init(HeroInfo heroInfo)
    {
        Debug.Log("Stage Init");

        this.heroinfo = heroInfo;
        DataManager.Instance.LoadAllData();
        var stageData = DataManager.Instance.dicStageData[this.heroinfo.stageLevel];


    }
}
