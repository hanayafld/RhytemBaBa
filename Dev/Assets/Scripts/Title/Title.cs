using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public System.Action OnTitleEnd;

    private HeroInfo heroInfo;

    private System.Action OnFadeInEnd;
    private System.Action OnFadeOutEnd;

    public Image img_dim;
    public Button btn_continue;
    public Button btn_newGame;

    void Start()
    {
        Debug.Log("Title Start");

        //인포 로드
        var loadInfo = InfoManager.Instance.LoadInfo();

        //인포가 있으면, 이어하기 버튼 활성화
        if (loadInfo)
        {
            this.heroInfo = InfoManager.Instance.heroInfo;
            this.btn_continue.gameObject.SetActive(true);
        }
        else
        {
            this.btn_continue.gameObject.SetActive(false);
        }

        this.OnFadeInEnd = () =>
          {
              //준비 완료
              this.Ready2Game();
          };

        StartCoroutine(this.FadeIn());
    }

    private void Ready2Game()
    {
        this.btn_continue.onClick.AddListener(() =>
        {
            Debug.Log("이어하기");

        });

        this.btn_newGame.onClick.AddListener(() =>
        {
            Debug.Log("새로 시작");
            InfoManager.Instance.CreateInfo();
            this.heroInfo = InfoManager.Instance.heroInfo;
            var stageLevel = this.heroInfo.stageLevel;
            this.InGameInit();
        });
    }

    private void InGameInit()
    {
        var operInGame = SceneManager.LoadSceneAsync("InGame");
        operInGame.completed += (AsyncOperation) =>
          {
              Debug.Log("InGame Load Scene");
              var inGame = GameObject.FindObjectOfType<InGame>();
              inGame.OnInGameEnd = () =>
                {
                    this.OnTitleEnd();
                };
          };
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
            alpha += 0.016f;
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
