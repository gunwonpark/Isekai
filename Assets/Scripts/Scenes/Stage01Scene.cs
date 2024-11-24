using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage01Scene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.stage01;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
	}

	public override void Clear()
	{

	}
}
