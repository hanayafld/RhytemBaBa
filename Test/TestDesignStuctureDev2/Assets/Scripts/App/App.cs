using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    private System.Action OnStartTitle;
    private System.Action OnStartLogo;

    private void Awake()
    {
        //이 씬을 제거하지 않음
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        #region 씬 전환(Logo)
        //로고 씬 불러오기

        this.OnStartLogo = () =>
        {
            var operLogo = SceneManager.LoadSceneAsync("Logo");
            operLogo.completed += (AsyncOperation) =>
            {
                Debug.Log("Logo 씬 로드");
                var logo = GameObject.FindObjectOfType<Logo>();
                logo.OnLogoSceneEnd = () =>
                {
                    //로고 씬 끝나면 타이틀 씬 불러오기
                    this.OnStartTitle();
                };
                logo.Init();

            };
        };

        this.OnStartTitle = () =>
        {
            //타이틀 씬 불러오기
            var operTitle = SceneManager.LoadSceneAsync("Title");
            operTitle.completed += (AsyncOperation) =>
             {
                 Debug.Log("Title 씬 로드");
                 var title = GameObject.FindObjectOfType<Title>();
                 title.OnTitleSceneEnd = () =>
                 {
                     this.OnStartLogo();
                 };
                 title.Init();
                 //타이틀로 돌아가기
             };
        };
        
        this.OnStartLogo();
        #endregion
    }
}
