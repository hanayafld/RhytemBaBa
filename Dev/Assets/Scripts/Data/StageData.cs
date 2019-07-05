using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : RawData
{
    //스테이지 이름
    public string stageName;
    //프리팹 경로
    public string prefabPath;
    //기본 음악
    public float bgm_BPM;
    public int bgm_Length;
    public int bgm_Type;
    public int bgm_Standby;
    //보스 음악
    public float bgm_bossBPM;
    public int bgm_bossLength;
    public int bgm_bossType;
    public int bgm_bossStandby;
}
