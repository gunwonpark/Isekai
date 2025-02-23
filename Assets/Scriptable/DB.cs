using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LoadingSceneData
{
    public string name;
    public WorldType worldType;
    public string tip;
}

[System.Serializable]
public struct RealGameSceneData
{
    public WorldType worldType;
    public float cameraSpeed;
}

[CreateAssetMenu(fileName = "DB", menuName = "ScriptableObject/DB", order = 0)]
public class DB : ScriptableObject
{
    [SerializeField] private List<LoadingSceneData> loadingTipDataList = new();
    private Dictionary<WorldType, LoadingSceneData> loadingTipDataDic = new();

    [SerializeField] private List<RealGameSceneData> realGameSceneDataList = new();
    private Dictionary<WorldType, RealGameSceneData> realGameSceneDataDic = new(); 

    public void Init()
    {
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
    }

    public LoadingSceneData GetLoadingSceneData(WorldType worldType)
    {
        if(loadingTipDataDic.TryGetValue(worldType, out LoadingSceneData data))
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

}
