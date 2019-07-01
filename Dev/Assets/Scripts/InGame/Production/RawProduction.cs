using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawProduction : MonoBehaviour
{
    public System.Action OnProductionSkip;

    public Button btn_skip;

    void Start()
    {
        this.btn_skip.onClick.AddListener(() =>
        {
            Debug.Log("연출 스킵");
            this.OnProductionSkip();
        });
    }
}
