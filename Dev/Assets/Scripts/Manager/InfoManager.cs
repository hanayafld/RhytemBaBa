
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class InfoManager : MonoBehaviour
{
    [HideInInspector]
    public static InfoManager Instance;
    public HeroInfo heroInfo;

    private string path = "/HeroInfo.json";
    
    void Awake()
    {
        InfoManager.Instance = this;
    }

    public bool LoadInfo()
    {
        if (File.Exists(Application.persistentDataPath+path))
        {
            Debug.Log("Load Info");
            var json = File.ReadAllText(Application.persistentDataPath+path);
            var heroInfo = JsonConvert.DeserializeObject<HeroInfo>(json);
            this.heroInfo = heroInfo;

            return true;
        }
        else
        {
            Debug.Log("Info가 없습니다 NewGame을 하세요.");

            return false;
        }
    }

    public void SaveInfo(HeroInfo heroInfo)
    {
        Debug.Log("Save Info");

        this.heroInfo = heroInfo;
        var json = JsonConvert.SerializeObject(this.heroInfo);
        File.WriteAllText(Application.persistentDataPath+this.path, json);
    }

    public void CreateInfo()
    {
        Debug.Log("Create Info");

        var data = DataManager.Instance;
        data.LoadAllData();

        this.heroInfo = new HeroInfo();

        this.heroInfo.id = 0;
        this.heroInfo.stageLevel = 0;
        this.heroInfo.maxHp = data.dicHeroData[0].defaultHp;
        this.heroInfo.damage = 1;
        this.heroInfo.gold = 3;
        //this.heroInfo.artifacts = null;
    }
}
