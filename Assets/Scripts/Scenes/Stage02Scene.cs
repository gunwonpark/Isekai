using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage02Scene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.stage02;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
	}

	public override void Clear()
	{

	}
}
