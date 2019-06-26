using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class InfoManager : MonoBehaviour
{
    [HideInInspector]
    public static InfoManager Instance;

    [HideInInspector]
    public HeroInfo heroInfo;

    [HideInInspector]
    public OptionInfo optionInfo;

    void Start()
    {
        InfoManager.Instance = this;
    }

    public bool LoadInfo()
    {
        this.heroInfo = new HeroInfo();

        if (File.Exists(Application.dataPath + "/Info/HeroInfo.json"))
        {
            Debug.Log("파일을 찾았습니다.");
            var json = File.ReadAllText(Application.dataPath + "/Info/HeroInfo.json");
            var heroInfo = JsonConvert.DeserializeObject<HeroInfo>(json);
            this.heroInfo = heroInfo;
            return true;
        }
        else
        {
            Debug.Log("파일을 찾지 못했습니다. 신규 생성 해주세요.");
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Info");

            if (directoryInfo.Exists == false)
            {
                directoryInfo.Create();
            }

            this.heroInfo = null;
            return false;
        }
    }

    public HeroInfo CreateInfo()
    {
        Debug.Log("Info 생성");
        this.heroInfo = new HeroInfo();
        this.heroInfo.stageLevel = 0;
        //this.heroInfo.maxHp = heroData.defaultHp;
        this.heroInfo.damage = 1;
        this.heroInfo.gold = 5;
        this.heroInfo.kill = 0;
        this.heroInfo.artifacts = new List<int>();
        
        return this.heroInfo;
    }
    
    public void SaveInfo()
    {
        //Application.dataPath + "/Info/HeroInfo.json"

    }

    public void DeleteInfo()
    {
        //Application.dataPath + "/Info/HeroInfo.json"
        
    }
}
