using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage03Scene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.stage03;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
	}

	public override void Clear()
	{

	}
}
