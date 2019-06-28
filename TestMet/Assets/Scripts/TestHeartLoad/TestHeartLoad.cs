using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TestHeartLoad : MonoBehaviour
{
    public Button btn_AddHeart;
    public Button btn_RemoveHeart;
    public Button btn_Hit;
    public Button btn_Heal;

    public int hp;
    public int hpMax;

    public List<Heart> Hearts;

    

    void Start()
    {
        this.hpMax = 1;
        this.hp = 1;
        this.SortHp();

        this.btn_AddHeart.onClick.AddListener(() =>
        {
            if (this.hpMax < this.Hearts.Count)
            {
                this.hp++;
                this.hpMax++;
            }
            else
            {
                Debug.Log("체력을 더 늘릴 수 없습니다.");
            }

            this.SortHp();
            Debug.LogFormat("hp : {0}/{1}", this.hp, this.hpMax);
        });

        this.btn_RemoveHeart.onClick.AddListener(() =>
        {
            //리스트의 마지막칸에 있는 하트를 잃어라?
            if (this.hpMax > 1)
            {
                this.hp--;
                this.hpMax--;
            }
            else
            {
                Debug.Log("체력은 1보다 작을 수 없습니다.");
            }

            this.SortHp();

            Debug.LogFormat("hp : {0}/{1}", this.hp, this.hpMax);
        });

        this.btn_Hit.onClick.AddListener(() =>
        {
            //데미지1
            if (this.hp > 1)
            {
                this.hp--;
            }
            else if(this.hp==1)
            {
                this.hp--;
                Debug.Log("사망!");
            }

            this.SortHp();

            Debug.LogFormat("hp : {0}/{1}", this.hp, this.hpMax);
        });

        this.btn_Heal.onClick.AddListener(() =>
        {
            //회복1
            if (this.hp < this.hpMax)
            {
                this.hp++;
            }
            else
            {
                Debug.Log("체력이 가득 찼습니다.");
            }

            this.SortHp();
            Debug.LogFormat("hp : {0}/{1}", this.hp, this.hpMax);
        });
    }

    private void SortHp()
    {
        for (int i = 0; i < this.Hearts.Count; i++)
        {
            this.Hearts[i].img_hp.gameObject.SetActive(false);
            this.Hearts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < this.hpMax; i++)
        {
            this.Hearts[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < this.hp; i++)
        {
            this.Hearts[i].img_hp.gameObject.SetActive(true);
        }
    }
}
