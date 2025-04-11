using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : BaseScene
{
    private void Start()
    {
        ShowLodingUI();
    }

    private void ShowLodingUI()
    {
        if(Managers.Scene.prevSceneType == Scene.GameScene)
        {
        }
        else
        {
            // Test¿ë
            //Managers.UI.ShowSceneUI<UI_Loading>();  
            Managers.UI.ShowSceneUI<UI_GameLoading>();
        }
        
    }

    public override void Clear()
    {
        
    }
}
