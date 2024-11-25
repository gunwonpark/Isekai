using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdingScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.EndingScene;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
		Managers.Sound.Play("radioEffect", Sound.Bgm);
	}

	public override void Clear()
	{

	}
}
