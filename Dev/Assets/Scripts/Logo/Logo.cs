using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    public System.Action OnLogoEnd;

    private System.Action OnFadeInOutEnd;
    
    public Image img_dim;

    void Start()
    {
        Debug.Log("Logo Start");

        //페이드 인 아웃 끝나면
        this.OnFadeInOutEnd = () =>
        {
            //로고 씬 완료
            this.OnLogoEnd();
        };

        //로고 페이드 인 페이드 아웃
        StartCoroutine(this.FadeInOut());

    }

    private IEnumerator FadeInOut()
    {
        Debug.Log("Logo FadeInOut");
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

        yield return new WaitForSeconds(1);

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

        this.OnFadeInOutEnd();
    }
}
