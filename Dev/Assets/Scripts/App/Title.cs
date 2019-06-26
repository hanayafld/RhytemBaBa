using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    private System.Action OnStartCamp;
    private System.Action OnStartStage;
    private System.Action OnStartProduction;

    public Button btn_Continue;
    public Button btn_NewGame;
    public Button btn_Option;

    private HeroInfo heroInfo;

    void Start()
    {
        Debug.Log("Title 시작");

        //히어로 인포 로드
        var info = InfoManager.Instance.LoadInfo();
        this.heroInfo = InfoManager.Instance.heroInfo;
        //옵션 인포 로드
        //볼륨 설정



        //신규 -> 이어하기 버튼 비활성화
        //기존 -> 이어하기 버튼 활성화
        if (info)
        {
            Debug.Log("Info가 존재합니다.");
            this.btn_Continue.gameObject.SetActive(true);
        }

        //페이드 인아웃
        //끝나면
        this.Ready2Start();
    }

    private void Ready2Start()
    {
        //이어하기
        btn_Continue.onClick.AddListener(() =>
        {
            Debug.Log("이어하기");
            //인포에 맞는 캠프 불러오기
            this.StartGame();
            //인게임 씬에서 
        });

        //새로하기
        btn_NewGame.onClick.AddListener(() =>
        {
            Debug.Log("뉴게임");
            this.heroInfo = InfoManager.Instance.CreateInfo();
            //튜토리얼 진행하기
            this.StartGame();
            //인게임 씬에서 튜토리얼 진행하기
        });

        ////모든 저장은 캠프 초입에서 합니다.

        //옵션
        btn_Option.onClick.AddListener(() =>
        {
            //옵션 인포 불러오고
            Debug.Log("옵션");
            //옵션 인포에서 볼륨 조절하면 조절되고 바로 저장하기.
        });
    }

    private void StartGame()
    {
        Debug.LogFormat("게임시작 stageLevel : {0}", this.heroInfo.stageLevel);

        //캠프 시작
        this.OnStartCamp = () => 
        {

        };

        //스테이지 시작
        this.OnStartStage = () => 
        {

        };

        //연출 시작
        this.OnStartProduction = () => 
        {

        };

        //씬 로드
        if (this.heroInfo.stageLevel == 0)
        {
            //인트로 시작
            //인트로 끝나면 튜토리얼(Stage00)
            this.OnStartStage();
        }



    }
}
