using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : BaseScene
{
	[SerializeField] AudioSource _bgm;
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.EndingScene;
		//Managers.UI.ShowSceneUI<UI_GameScene>();
		_bgm.time = 6f;
		_bgm.Play();
    }

	public override void Clear()
	{

	}
}
