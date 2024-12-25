using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.TitleScene;
		Managers.UI.ShowSceneUI<UI_TitleScene>();
	}

	public override void Clear()
	{
		Debug.Log("LoginScene Clear!");
	}
}
