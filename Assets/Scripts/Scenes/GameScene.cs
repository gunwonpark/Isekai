using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.Game;
		//Managers.UI.ShowSceneUI<UI_GameScene>();

	}

	public override void Clear()
	{

	}
}
