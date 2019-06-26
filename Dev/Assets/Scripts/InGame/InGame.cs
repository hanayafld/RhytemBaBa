using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame : MonoBehaviour
{
    //새로시작이냐, 아니면 이어하기냐
    public HeroInfo heroInfo;

    void Start()
    {
        //인포 로드
        InfoManager.Instance.LoadInfo();
        this.heroInfo = InfoManager.Instance.heroInfo;
    }

    public void Continue()
    {
        Debug.Log("Continue");
        //스테이지 불러오기
    }

    public void NewGame()
    {
        Debug.Log("NewGame");
        //인트로 불러오기
    }

    //스테이지 구조
    //인트로 -> 튜토리얼(Stage00) -> 연출(전부 미정)
    //-> 캠프 -> 스테이지 -> 연출(반복)
    //-> 엔딩

    public void StartGame()
    {
        

        if (this.heroInfo.stageLevel == 0)
        {
            //인트로
        }
        else
        {
            //해당하는 캠프로 이동
        }


    }
}
