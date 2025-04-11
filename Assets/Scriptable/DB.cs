using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LoadingSceneData
{
    public string name;
    public WorldType worldType;
    public string tip;
    public Sprite backgroundImage;
    public List<Sprite> subImages;
}

[System.Serializable]
public struct RealGameSceneData
{
    public WorldType worldType;
    public float cameraSpeed;
}

[System.Serializable]
public struct GameSceneData
{
    public WorldType worldType;
    public List<int> difficulty;
    public List<int> startGauge;
    public List<int> perDecreaseGauge;
    public List<int> perIncreaseGauge;
    public List<int> succedGauge;
    public List<int> failGauge;
    public List<int> runGauge;
    public List<int> limitTime;
    public List<string> dialog;
    public List<KeyCode> requireKeys;
    public List<int> requiredKeyCount;
    public List<bool> canPressConcurrent;
}

[System.Serializable]
public struct PlayerData
{
    public List<float> moveSpeed;
}

[System.Serializable]
public struct LoadingGameSceneData
{
    public WorldType worldType;
    public List<string> todoList;
    public string diary;
    public string startDate;
    public string endDate;
    public string worldName;
}

[System.Serializable]
public struct EndingSceneData
{
    public List<string> newsDialog;
    public List<string> finalDialog;
}

[CreateAssetMenu(fileName = "DB", menuName = "ScriptableObject/DB", order = 0)]
public class DB : ScriptableObject
{
    [SerializeField] private List<LoadingSceneData> loadingTipDataList = new();
    private Dictionary<WorldType, LoadingSceneData> loadingTipDataDic = new();

    [SerializeField] private List<RealGameSceneData> realGameSceneDataList = new();
    private Dictionary<WorldType, RealGameSceneData> realGameSceneDataDic = new();

    [SerializeField] private List<GameSceneData> gameSceneDataList = new();
    private Dictionary<WorldType, WorldInfo> gameSceneDataDic = new();

    [SerializeField] private PlayerData playerData = new();
    private PlayerData _sudoPlayerData = new();

    [SerializeField] private List<LoadingGameSceneData> loadingGameSceneDataList = new();
    private Dictionary<WorldType, LoadingGameSceneData> loadingGameSceneDataDic = new();

    [SerializeField] private EndingSceneData endingSceneData;

    [SerializeField] private List<SoundData> soundDatas = new();
    private Dictionary<string, SoundData> soundDataDic = new();


    public void Init()
    {
//        #region 치트수정 방지용
//#if Unity_Editor
//        string savePath = Application.dataPath + "/Datas";
//        string path = System.IO.Path.Combine(savePath, "DB.json");
//        string json = System.IO.File.ReadAllText(path);

//        JsonUtility.FromJsonOverwrite(json, this);
//#endif
//        #endregion

        loadingTipDataDic.Clear();
        foreach (var data in loadingTipDataList)
        {
            loadingTipDataDic.Add(data.worldType, data);
        }

        realGameSceneDataDic.Clear();
        foreach (var data in realGameSceneDataList)
        {
            realGameSceneDataDic.Add(data.worldType, data);
        }

        gameSceneDataDic.Clear();
        foreach (var data in gameSceneDataList)
        {
            switch (data.worldType)
            {
                case WorldType.Pelmanus:
                    gameSceneDataDic.Add(data.worldType, new PelmanusWorldInfo(data));
                    break;
                case WorldType.Vinter:
                    gameSceneDataDic.Add(data.worldType, new VinterWorldInfo(data));
                    break;
                case WorldType.Chaumm:
                    gameSceneDataDic.Add(data.worldType, new ChaummWorldInfo(data));
                    break;
                case WorldType.Gang:
                    gameSceneDataDic.Add(data.worldType, new GangWorldInfo(data));
                    break;
            }
        }

        loadingGameSceneDataDic.Clear();
        foreach (var data in loadingGameSceneDataList)
        {
            loadingGameSceneDataDic.Add(data.worldType, data);
        }

        // 데이터 참조지역 추후 초기화
        _sudoPlayerData = playerData;
    }

    public LoadingSceneData GetLoadingSceneData(WorldType worldType)
    {
        if (loadingTipDataDic.TryGetValue(worldType, out LoadingSceneData data))
        {
            return data;
        }

        return new LoadingSceneData();
    }

    public RealGameSceneData GetRealGameSceneData(WorldType worldType)
    {
        if (realGameSceneDataDic.TryGetValue(worldType, out RealGameSceneData data))
        {
            return data;
        }
        return new RealGameSceneData();
    }

    public EndingSceneData GetEndingSceneData()
    {
        return endingSceneData;
    }

    public WorldInfo GetGameSceneData(WorldType worldType)
    {
        if (gameSceneDataDic.TryGetValue(worldType, out WorldInfo data))
        {
            return data;
        }

        return null;
    }

    public PlayerData GetPlayerData()
    {
        return _sudoPlayerData;
    }

    public LoadingGameSceneData GetLoadingGameSceneData(WorldType worldType)
    {
        if (loadingGameSceneDataDic.TryGetValue(worldType, out LoadingGameSceneData data))
        {
            return data;
        }
        return new LoadingGameSceneData();
    }

    // 전역데이터 수정 방지용
    public void SetGameSceneData(WorldType worldType, GameSceneData data)
    {
        switch (data.worldType)
        {
            case WorldType.Pelmanus:
                gameSceneDataDic[worldType] = new PelmanusWorldInfo(data);
                break;
            case WorldType.Vinter:
                gameSceneDataDic[worldType] = new VinterWorldInfo(data);
                break;
            case WorldType.Chaumm:
                gameSceneDataDic[worldType] = new ChaummWorldInfo(data);
                break;
            case WorldType.Gang:
                gameSceneDataDic[worldType] = new GangWorldInfo(data);
                break;
        }
    }

    public void SetPlayerData(PlayerData data)
    {
        _sudoPlayerData = data;
    }

    public void ResetPlayerData()
    {
        _sudoPlayerData = playerData;
    }
}
