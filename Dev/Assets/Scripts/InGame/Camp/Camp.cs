using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camp : MonoBehaviour
{
    public System.Action OnCampEnd;

    private System.Action OnFadeInEnd;
    private System.Action OnFadeOutEnd;
    
    private HeroInfo heroInfo;
    private Dictionary<int, CampData> dicCampData;

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
        this.dicCampData = DataManager.Instance.dicCampData;
        
        //캠프 프리팹 불러오기
        this.LoadPrefab();


        //페이드 인
        this.OnFadeInEnd = () =>
        {
            //상인걸어오는 연출 다 걸어오면 상인 앉아서 대기
            
            //나중에 만들 것//상인이 앉으면 상점이용

            //상인 누르면 페이드 아웃되며 상인 일어나 밖으로 걸어가기// 스테이지 시작


        };

        this.OnFadeOutEnd = () =>
        {
            //스테이지로 이동
        };
        StartCoroutine(this.FadeIn());
    }

    //프리팹 불러오기
    private void LoadPrefab()
    {
        var campPrefab = Resources.Load<GameObject>(this.dicCampData[this.heroInfo.stageLevel].prefabPath);
        var camp = Instantiate(campPrefab).GetComponent<CampPrefab>();

        //this.hero = camp.hero;
        //this.trader = camp.trader;
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

        this.OnFadeInEnd();
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
