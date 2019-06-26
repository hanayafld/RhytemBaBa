using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfo : MonoBehaviour
{
    public int stageLevel;//스테이지 레벨, 로드시 이용.
    public int maxHp;//Hp
    public int damage;//데미지
    public int gold;
    public List<int> artifacts;//보유한 아티펙트의 아이디들을 수집

    public int kill;
}
