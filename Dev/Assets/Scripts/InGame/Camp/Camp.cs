using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camp : MonoBehaviour
{
    public System.Action OnCampEnd;
    
    private System.Action OnFadeOutEnd;

    private HeroInfo heroInfo;
    private CampData campData;

    public Image img_dim;

    private Hero hero;
    private Trader trader;

    public void Init(HeroInfo heroInfo)
    {
        this.heroInfo = heroInfo;
        //인포 세이브
        InfoManager.Instance.SaveInfo(this.heroInfo);

        //데이터 불러오기
        DataManager.Instance.LoadAllData();
        this.campData = DataManager.Instance.dicCampData[this.heroInfo.stageLevel];

        //캠프 프리팹 불러오기
        this.LoadPrefab();

        this.trader.OnComeEnd = () => 
        {
            this.Ready2Shopping();
        };

        this.OnFadeOutEnd = () =>
        {
            //스테이지로 이동
            this.OnCampEnd();
        };

        //페이드 인
        //this.hero.anim(앉기);
        StartCoroutine(this.FadeIn());
        StartCoroutine(this.trader.Come());
    }

    //프리팹 불러오기
    private void LoadPrefab()
    {
        var campPrefab = Resources.Load<GameObject>(this.campData.prefabPath);
        var camp = Instantiate(campPrefab).GetComponent<CampPrefab>();

        this.hero = camp.hero;
        this.trader = camp.trader;
    }

    private void Ready2Shopping()
    {
        Debug.Log("Ready2Shopping");

        //상인 버튼 활성화
        this.trader.btn_trade.gameObject.SetActive(true);
        //대화 버블 활성화
        this.trader.speechBubble.SetActive(true);

        this.trader.btn_trade.onClick.AddListener(() =>
        {
            Debug.Log("상점 버튼 누름");

            this.trader.btn_trade.gameObject.SetActive(false);
            this.trader.speechBubble.SetActive(false);

            //나중에 추가할 것
            //show UIPopup
            //상점열기 밑 아이템구매

            //Test용, 스테이지 진입
            StartCoroutine(this.FadeOut());
        });
    }


    #region FadeInOut
    private IEnumerator FadeIn()
    {
        var color = this.img_dim.color;
        float alpha = color.a;

        while (true)
        {
            alpha -= 0.016f;
            color.a = alpha;
            this.img_dim.color = color;
            if (alpha < 0)
            {
                break;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        var color = this.img_dim.color;
        float alpha = color.a;

        while (alpha < 1)
        {
            alpha += 0.008f;
            color.a = alpha;
            this.img_dim.color = color;
            if (alpha > 1)
            {
                break;
            }
            yield return null;
        }

        this.OnFadeOutEnd();
    }
    #endregion
}
