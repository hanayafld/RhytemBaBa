using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public System.Action OnTitleSceneEnd;

    public Button btn_Continew;
    public Button btn_NewGame;
    public Button btn_Option;

    //인포 매니져에서 받아올 스테이지 레벨
    private HeroInfo heroInfo;

    public void Init()
    {
        Debug.Log("Title Init");
        //GPGS연동
        //Data 불러오기
        var data = DataManager.Instance;
        data.LoadAllDatas();

        //Info 불러오기
        //true 면 기존유저, false면 신규유저
        var userCheck = InfoManager.Instance.LoadHeroInfo();

        if (userCheck)
        {
            //기존유저
            //이어하기 활성화
            this.btn_Continew.gameObject.SetActive(true);

            //히어로 인포 저장
            this.heroInfo = InfoManager.Instance.heroInfo;
        }
        else
        {
            //신규유저
            //이어하기 비 활성화
            this.btn_Continew.gameObject.SetActive(false);

        }
        
        //페이드 인 아웃
        //게임 시작 준비 호출
        this.Ready2theGame();

    }

    private void Ready2theGame()
    {
        Debug.Log("Ready2theGame");

        #region 씬 전환
        //버튼 3개

        this.btn_Continew.onClick.AddListener(() =>
        {
            Debug.Log("이어 하기");
            var stageLevel = this.heroInfo.stageLevel;
            //인게임 불러오기
            this.InGameInit(this.heroInfo);
        });

        this.btn_NewGame.onClick.AddListener(() =>
        {
            Debug.Log("새 게임");
            //인게임 불러오기
            InfoManager.Instance.CreateInfo();
            this.heroInfo = InfoManager.Instance.heroInfo;
            var stageLevel = this.heroInfo.stageLevel;
            this.InGameInit(this.heroInfo);
        });
        #endregion

        this.btn_Option.onClick.AddListener(() =>
        {
            Debug.Log("옵션");
            //옵션 팝업
        });
    }

    private void InGameInit(HeroInfo heroInfo)
    {
        var operInGame = SceneManager.LoadSceneAsync("InGame");
        operInGame.completed += (AsyncOperation) =>
          {
              Debug.Log("InGame 씬 로드");
              var inGame = GameObject.FindObjectOfType<InGame>();
              inGame.OnInGameSceneEnd = () =>
                {
                    //
                    this.OnTitleSceneEnd();
                };
              inGame.Init(this.heroInfo);
          };
    }
}
