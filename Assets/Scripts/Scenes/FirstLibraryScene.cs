using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLibraryScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.FirstLibraryScene;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
	}

	public override void Clear()
	{

	}
}
