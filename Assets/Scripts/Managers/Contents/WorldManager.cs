using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    public List<bool> isWorldClear;
    public WorldType CurrentWorldType
    {
        get;set;
    }


    public void Init()
    {
        isWorldClear = new List<bool>();
        for (int i = 0; i < (int)WorldType.Max; i++)
        {
            isWorldClear.Add(false);
        }
    }
}
