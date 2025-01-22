using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEx: BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.GameScene;

        VinterWorldInfo world = new VinterWorldInfo();
        Managers.UI.ShowPopupUI<UI_MiniGame>().Init(world);
		Managers.Sound.Play("anotherWorldBgm",Sound.Bgm);
	}

	public override void Clear()
	{

	}
}
