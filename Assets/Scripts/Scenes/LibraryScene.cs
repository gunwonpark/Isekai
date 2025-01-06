using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.LibraryScene;
		//Managers.UI.ShowPopupUI<UI_BookPopup>();
	}

	public override void Clear()
	{

	}
}
