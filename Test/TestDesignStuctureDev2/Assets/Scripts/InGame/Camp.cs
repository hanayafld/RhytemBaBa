using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Camp : MonoBehaviour, IPointerUpHandler
{
    public System.Action<HeroInfo> OnCampEnd;

    public Text txt_StageLevel;
    public Button btn_uiTradePopUp;
    public Button btn_item00;

    public GameObject ui_TradePopUp;
    public Button btn_StartStage;
    
    private HeroInfo heroInfo;

    public void Init(HeroInfo heroInfo)
    {
        Debug.Log("Camp Init");
        this.heroInfo = heroInfo;

        Debug.Log("캠프 인포 저장");
        InfoManager.Instance.SaveInfo(this.heroInfo);


        this.txt_StageLevel.text = string.Format("Camp : {0}", this.heroInfo.stageLevel);

        //인포에 현재 정보 저장하기
        

        //페이드 인 되며
        //상인 걸어옴
        //상인 앉음 상인 활성화
        this.btn_uiTradePopUp.gameObject.SetActive(true);
        //상인 누르면, 팝업
        this.btn_uiTradePopUp.onClick.AddListener(() =>
        {
            //팝업
            this.ui_TradePopUp.SetActive(true);
            //상점 이용기능 구현
            


        });
        //구매완료시, 팝업 종료
        this.btn_StartStage.onClick.AddListener(() =>
        {
            this.ui_TradePopUp.SetActive(false);

            //상인 왼쪽으로 걸어가며 페이드 아웃
            this.OnCampEnd(this.heroInfo);
        });
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        ((IPointerUpHandler)btn_item00).OnPointerUp(eventData);
    }
}
