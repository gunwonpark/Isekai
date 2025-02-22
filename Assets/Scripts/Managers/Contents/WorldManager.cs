using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    private WorldType _currentWorldType = WorldType.Pelmanus;
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
}
