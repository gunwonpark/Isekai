using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.GameScene;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
		Managers.Sound.Play("realWorldBgm", Sound.Bgm);
	}

	public override void Clear()
	{

	}
}
