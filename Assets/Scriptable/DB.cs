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

    [SerializeField] private List<WorldInfo> worldInfos = new();
    private Dictionary<WorldType, WorldInfo> worldInfoDic = new();

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

        foreach (WorldType type in System.Enum.GetValues(typeof(WorldType)))
        {
            switch (type)
            {
                case WorldType.Vinter:
                    worldInfos.Add(new VinterWorldInfo());
                    break;
                case WorldType.Gang:
                    worldInfos.Add(new GangWorldInfo());
                    break;
                case WorldType.Pelmanus:
                    worldInfos.Add(new PelmanusWorldInfo());
                    break;
                case WorldType.Chaumm:
                    worldInfos.Add(new ChaummWorldInfo());
                    break;
            }
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
