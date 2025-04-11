using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
	public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
	public Scene prevSceneType { get; private set; } = Scene.Unknown;

    public void LoadScene(Scene type)
	{
		Managers.Clear();

		prevSceneType = CurrentScene?.SceneType ?? Scene.Unknown;
        SceneManager.LoadScene(GetSceneName(type));
	}

	string GetSceneName(Scene type)
	{
		string name = System.Enum.GetName(typeof(Scene), type);
		return name;

    }

	public void Clear()
	{
		CurrentScene?.Clear();
	}
}
