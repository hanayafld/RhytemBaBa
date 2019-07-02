using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject hero;
    public GameObject enemy;

    public Button btn_idle;
    public Button btn_B_waiting;
    public Button btn_run;

    void Start()
    {
        var heroAnim = this.hero.GetComponentInChildren<Animator>();
        var enemyAnim = this.enemy.GetComponentInChildren<Animator>();

        this.btn_idle.onClick.AddListener(() =>
        {
            heroAnim.Play("idle");
            enemyAnim.Play("LightGuard_Idle");
        });

        this.btn_B_waiting.onClick.AddListener(() =>
        {
            heroAnim.Play("B_waiting");
            enemyAnim.Play("LightGuard_CombatIdle");
        });

        this.btn_run.onClick.AddListener(() =>
        {
            heroAnim.Play("run");
            enemyAnim.Play("LightGuard_Run");
        });
    }
}
