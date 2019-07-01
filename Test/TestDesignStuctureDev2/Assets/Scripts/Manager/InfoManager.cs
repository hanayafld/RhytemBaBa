using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class InfoManager : MonoBehaviour
{
    public static InfoManager Instance;

    [HideInInspector]
    public HeroInfo heroInfo;
    //옵션인포
    [HideInInspector]
    public OptionInfo optionInfo;

    #region Path
    private string heroInfoPath = "Info/HeroInfo.json";
    private string optionInfoPath = "Info/OptionInfo.json";
    #endregion

    void Awake()
    {
        InfoManager.Instance = this;
    }

    public bool LoadHeroInfo()
    {
        this.heroInfo = new HeroInfo();

        if (File.Exists(heroInfoPath))
        {
            Debug.Log("파일을 찾았습니다.");
            var json = File.ReadAllText(heroInfoPath);
            var heroInfo = JsonConvert.DeserializeObject<HeroInfo>(json);
            this.heroInfo = heroInfo;
            return true;
        }
        else
        {
            Debug.Log("파일을 찾지 못했습니다. 신규 생성 해주세요.");
            this.heroInfo = null;
            return false;
        }
    }

    public void LoadOptionInfo()
    {
        this.optionInfo = new OptionInfo();

        if (File.Exists(optionInfoPath))
        {
            Debug.Log("옵션인포 로드 완료.");
            var json = File.ReadAllText(optionInfoPath);
            var optionInfo = JsonConvert.DeserializeObject<OptionInfo>(json);
            this.optionInfo = optionInfo;
        }
        else
        {
            Debug.Log("옵션인포 정보 없음 인포 새로 생성");
            //옵션 인포 새로 생성
            this.optionInfo.BGM_Volume = 100;
            this.optionInfo.FX_Volume = 100;
        }
    }

    public void CreateInfo()
    {
        Debug.Log("신규 생성");

        var data = DataManager.Instance;
        data.LoadAllDatas();
        this.heroInfo = new HeroInfo();

        this.heroInfo.id = 0;
        this.heroInfo.stageLevel = 0;
        this.heroInfo.max_hp = data.dicHeroData[0].default_hp;
        this.heroInfo.damage = 1;
        this.heroInfo.gold = 5;
        this.heroInfo.artifacts = null;
    }

    public void SaveInfo(HeroInfo heroInfo)
    {
        Debug.Log("세이브 인포");
        
        this.heroInfo = heroInfo;
        var json = JsonConvert.SerializeObject(this.heroInfo);
        File.WriteAllText(this.heroInfoPath, json);
    }

    public void DeleteInfo()
    {
        File.Delete(this.heroInfoPath);
    }
}
