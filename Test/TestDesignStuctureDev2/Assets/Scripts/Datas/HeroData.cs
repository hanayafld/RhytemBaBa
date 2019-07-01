using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData : RawData
{
    public int default_hp;

    #region 애니메이션
    //대기모션
    public string anim_idle;
    public string anim_combatidle;

    //공격모션
    public string anim_attack00;
    public string anim_attack01;
    public string anim_attack02;
    public string anim_attack03;

    //회피모션
    public string anim_evasion00;
    public string anim_evasion01;
    public string anim_evasion02;
    public string anim_evasion03;

    //피해모션
    public string anim_hurt;

    //죽음
    public string anim_death;
    #endregion
}
