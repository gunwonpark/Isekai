using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEx: BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.GameScene;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
		Managers.Sound.Play("anotherWorldBgm",Sound.Bgm);
	}

	public override void Clear()
	{

	}
}
