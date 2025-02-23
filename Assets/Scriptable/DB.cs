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

[CreateAssetMenu(fileName = "DB", menuName = "ScriptableObject/DB", order = 0)]
public class DB : ScriptableObject
{
    [SerializeField] private List<LoadingSceneData> loadingTipDataList = new();
    private Dictionary<WorldType, LoadingSceneData> loadingTipDataDic = new();

    public void Init()
    {
        loadingTipDataDic.Clear();
        foreach (var data in loadingTipDataList)
        {
            loadingTipDataDic.Add(data.worldType, data);
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

}
