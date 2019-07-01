using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionInfo : MonoBehaviour
{
    //배경음
    [Range(0, 100)]
    public int BGM_Volume;
    //효과음
    [Range(0, 100)]
    public int FX_Volume;
}
