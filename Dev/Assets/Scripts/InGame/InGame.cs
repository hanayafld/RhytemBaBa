using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame : MonoBehaviour
{
    public System.Action OnInGameEnd;

    private System.Action OnStartCamp;
    private System.Action OnStartStage;
    private System.Action OnStartProduction;

    private HeroInfo heroInfo;


    void Awake()
    {
        //타이틀로 돌아가기 전까지 이 오브젝트를 파괴하지 않음
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        //인포 불러오기
        InfoManager.Instance.LoadInfo();
        this.heroInfo = InfoManager.Instance.heroInfo;

        #region Scene 전환
        this.OnStartCamp = () =>
        {
            //캠프 불러오기
            Debug.Log("Camp Scene Load");
            var operCamp = SceneManager.LoadSceneAsync("Camp");
            operCamp.completed += (AsyncOperation) =>
              {
                  var camp = FindObjectOfType<Camp>();
                  camp.OnCampEnd = () =>
                    {
                        this.OnStartStage();
                    };
                  camp.Init(this.heroInfo);
              };
        };

        this.OnStartStage = () =>
        {
            //스테이지 불러오기//스테이지 00 은 튜토리얼
            Debug.Log("Stage Scene Load");
            if (this.heroInfo.stageLevel == 0)
            {
                //튜토리얼
                Debug.Log("Tutorial Scene Load");
                var operTutorial = SceneManager.LoadSceneAsync("Tutorial");
                operTutorial.completed += (AsyncOperation) =>
                {
                    var tutorial = FindObjectOfType<Tutorial>();
                    tutorial.OnTutorialEnd = () =>
                    {
                        this.OnStartProduction();
                    };
                };
            }
            else
            {
                //스테이지 불러오기
                var operStage = SceneManager.LoadSceneAsync("Stage");
                operStage.completed += (AsyncOperation) =>
                {
                    var stage = FindObjectOfType<Stage>();
                    stage.OnStageEnd = () =>
                    {
                        this.OnStartProduction();
                    };
                };
            }
        };

        this.OnStartProduction = () =>
        {
            //연출 불러오기
            Debug.Log("Production Scene Load");
        };
        #endregion

        //StartCoroutine(this.Touch)

        //신규생성
        if (this.heroInfo.stageLevel == 0)
        {
            //인트로 불러오고
            Debug.Log("Intro Scene Load");
            var operIntro = SceneManager.LoadSceneAsync("Intro");
            operIntro.completed += (AsyncOperation) =>
              {
                  var intro = FindObjectOfType<Intro>();
                  intro.OnProductionSkip = () =>
                  {
                      //인트로 끝나면 Stage00시작
                      this.OnStartStage();
                  };
              };
        }
        else//신규 유저가 아니라면 캠프에서 시작
        {
            this.OnStartCamp();
        }   
    }
}
