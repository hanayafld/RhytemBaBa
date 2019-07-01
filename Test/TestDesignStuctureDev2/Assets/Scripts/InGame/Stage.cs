using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    #region Data
    private Dictionary<int, StageData> dicStageData = new Dictionary<int, StageData>();
    #endregion
    
    public System.Action<HeroInfo> OnStageClear;
    public System.Action OnStageRetry;
    public System.Action OnStageReturn;

    public Text txt_StageLevel;

    public Button btn_StageClear;
    public Button btn_Defeat;

    public GameObject ui_Defeat;
    public Button btn_Retry;
    public Button btn_Return;

    #region StagePrefab
    private GameObject[] arrMap;
    private AudioSource bg_Music;
    #endregion

    private HeroInfo heroInfo;
    private int heroHp;

    public void Init(HeroInfo heroInfo)
    {
        Debug.Log("Stage Init");

        #region 준비
        this.heroInfo = heroInfo;//매개변수로 넘어온 히어로 인포 이용
        this.heroHp = this.heroInfo.max_hp;

        this.txt_StageLevel.text = string.Format("Stage : {0}", this.heroInfo.stageLevel);//레벨 띄우기, 나중에 제거 혹은 변경
        //스테이지에서 제일 먼저 할것, 스테이지프리팹 불러오기
        DataManager.Instance.LoadAllDatas();//데이터 로드
        this.dicStageData = DataManager.Instance.dicStageData;//데이터 저장

        this.LoadStagePrefab();//스테이지 프리팹 로드
        #endregion

        #region 씬전환
        //스테이지 클리어시 this.OnStageEnd();
        btn_StageClear.onClick.AddListener(() =>
        {
            this.OnStageClear(this.heroInfo);
        });

        //패배시 YouDied팝업 띄우고
        this.btn_Defeat.onClick.AddListener(() =>
        {
            this.ui_Defeat.SetActive(true);
        });
        //btn_Restart시, this.OnStageRestart;
        this.btn_Retry.onClick.AddListener(() =>
        {
            //페이드 아웃하면서 ui_Defeat SetActive(false);
            this.ui_Defeat.SetActive(false);

            this.OnStageRetry();
        });

        //btn_Return시, this.OnStageReturn;
        this.btn_Return.onClick.AddListener(() =>
        {
            //페이드 아웃하면서 ui_Defeat SetActive(false);
            this.ui_Defeat.SetActive(false);

            this.OnStageReturn();
        });
        #endregion


        #region 게임 기능

        //이동, 전투 반복
        //BGM시작, 메트로놈 같이 시작
        //적 조우시 전투

        #endregion

    }

    private void LoadStagePrefab()
    {
        var stageGo = Resources.Load<GameObject>(this.dicStageData[this.heroInfo.stageLevel].stage_PreFabPath);
        var stage = Instantiate(stageGo).GetComponent<StagePrefab>();
        this.bg_Music = stage.bg_Music;//뮤직넘겨주기
        this.arrMap = stage.arrMaps;//맵넘겨주기
        //몬스터 그룹 넘겨주기
    }
}
