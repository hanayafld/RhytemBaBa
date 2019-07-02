using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public Dictionary<int, HeroData> dicHeroData;
    public Dictionary<int, CampData> dicCampData;
    //public Dictionary<int, StageData> dicStageData;
    //public Dictionary<int, MonsterData> dicMonsterData;

    void Awake()
    {
        DataManager.Instance = this;
    }

    void Start()
    {
        this.dicHeroData = new Dictionary<int, HeroData>();
        this.dicCampData = new Dictionary<int, CampData>();
        //this.dicStageData = new Dictionary<int, StageData>();
        //this.dicMonsterData = new Dictionary<int, MonsterData>();
    }

    public void LoadAllData()
    {
        this.dicHeroData = this.LoadData<HeroData>("Data/HeroData");//히어로 데이터
        this.dicCampData = this.LoadData<CampData>("Data/CampData");//캠프 데이터
        //this.dicStageData = this.LoadData<StageData>("Data/StageData");
        //this.dicMonsterData = this.LoadDAta<MonsterData>("Data/MonsterData");
    }

    private Dictionary<int, T> LoadData<T>(string path) where T : RawData
    {
        var data = Resources.Load<TextAsset>(path);
        var arrData = JsonConvert.DeserializeObject<T[]>(data.text);
        var dicData = arrData.ToDictionary(x => x.id, x => (T)x);
        return dicData;
    }
}
