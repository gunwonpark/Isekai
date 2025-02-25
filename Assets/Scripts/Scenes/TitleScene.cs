using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TitleScene : BaseScene
{
	//public VideoPlayer test;
	protected override void Init()
	{
		base.Init();
		//Screen.SetResolution(1920, 1080, true);
		SceneType = Scene.TitleScene;

		//fortest
		//string path = System.IO.Path.Combine(Application.streamingAssetsPath, "glitch.mp4");
		//test.url = path;

		Managers.World.CurrentWorldType = (WorldType)PlayerPrefs.GetInt("WorldType", 0);
    }

	public override void Clear()
	{
		Debug.Log("TitleScene Clear!");
	}
}
