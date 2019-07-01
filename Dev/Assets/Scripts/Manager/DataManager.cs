using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public Dictionary<int, HeroData> dicHeroData;

    void Awake()
    {
        DataManager.Instance = this;
    }

    void Start()
    {
        this.dicHeroData = new Dictionary<int, HeroData>();
    }

    public void LoadAllData()
    {
        this.dicHeroData = this.LoadData<HeroData>("Data/HeroData");
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
