using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.IntroScene;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
		Managers.Sound.Play("realWorldBgm", Sound.Bgm);
	}

	public override void Clear()
	{

	}
}
