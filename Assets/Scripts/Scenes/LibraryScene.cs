using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.LibraryScene;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
	}

	public override void Clear()
	{

	}
}
