using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCharacter : MonoBehaviour
{
    public GameObject chracterSprite;

    void Start()
    {
        //생성(랜덤 위치,  회전, 추락, 위로 리셋
        StartCoroutine(this.Falling());
        //포지션 리셋

    }

    private IEnumerator Falling()
    {
        float fallingSpeed = Random.Range(-0.15f, -0.03f);
        float rotateSpeed = Random.Range(-10, 10);
        while (true)
        {
            this.transform.Translate(new Vector3(0, fallingSpeed, 0));
            this.chracterSprite.transform.Rotate(new Vector3(0, 0, rotateSpeed));

            if (this.transform.localPosition.y < -25)
            {
                this.transform.localPosition = new Vector3(Random.Range(-9, 9), 20, 0);
            }
            yield return null;
        }
    }


}
