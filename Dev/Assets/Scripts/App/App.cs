using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    private System.Action OnLogoStart;
    private System.Action OnTitleStart;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        Debug.Log("App Start");
        #region Scene 전환
        //로고 불러오기
        this.OnLogoStart = () =>
        {
            var operLogo = SceneManager.LoadSceneAsync("Logo");
            operLogo.completed += (AsyncOperation) =>
            {
                Debug.Log("Logo Scene Load");
                var logo = GameObject.FindObjectOfType<Logo>();
                logo.OnLogoEnd = () =>
                  {
                      Debug.Log("Logo End");
                      this.OnTitleStart();
                  };
            };
        };

        //타이틀 불러오기
        this.OnTitleStart = () =>
        {
            var operTitle = SceneManager.LoadSceneAsync("Title");
            operTitle.completed += (AsyncOperation) =>
            {
                Debug.Log("Title Scene Load");
                var title = GameObject.FindObjectOfType<Title>();
                title.OnTitleEnd = () =>
                {
                    Debug.Log("Title End");
                    this.OnLogoStart();
                 };
            };
        };
        #endregion

        this.OnLogoStart();//로고 불러오기
        //this.OnTitleStart();//Test용 타이틀 바로 불러오기 로고씬 스킵가능
    }
}
