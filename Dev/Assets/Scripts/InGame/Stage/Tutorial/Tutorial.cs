using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public System.Action OnTutorialEnd;

    public Button btn_skip;

    void Start()
    {
        Debug.Log("Tutorial Start");

        this.btn_skip.onClick.AddListener(() =>
        {
            Debug.Log("Tutorial Skip");
            this.OnTutorialEnd();
        });
        
    }
}
