using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    private WorldType _currentWorldType = WorldType.Gang;
    public List<bool> isWorldClear;
    public WorldType CurrentWorldType
    {
        get { return _currentWorldType; }
        set { _currentWorldType = value; }
    }
    public void Init()
    {
        isWorldClear = new List<bool>();
        for (int i = 0; i < (int)WorldType.Max; i++)
        {
            isWorldClear.Add(false);
        }
    }
    public void WorldClear()
    {
        isWorldClear[(int)CurrentWorldType] = true;
    }
    public void MoveNextWorld()
    {
        CurrentWorldType++;
    }
    public WorldInfo GetWorldInfo()
    {
        WorldInfo worldInfo = null;

        switch (_currentWorldType)
        {
            case WorldType.Pelmanus:
                worldInfo = Managers.DB.GetGameSceneData(WorldType.Pelmanus) as PelmanusWorldInfo;
                break;
            case WorldType.Vinter:
                worldInfo = Managers.DB.GetGameSceneData(WorldType.Vinter) as VinterWorldInfo;
                break;
            case WorldType.Chaumm:
                worldInfo = Managers.DB.GetGameSceneData(WorldType.Chaumm) as ChaummWorldInfo;
                break;
            case WorldType.Gang:
                worldInfo = Managers.DB.GetGameSceneData(WorldType.Gang) as GangWorldInfo;
                break;
            default:
                break;
        }
        return worldInfo;
    }
}
