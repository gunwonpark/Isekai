using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LibraryTimeLineEndCallback : MonoBehaviour
{
	[SerializeField] PlayableDirector _playableDirector;

	private void OnEnable()
	{
		_playableDirector.stopped += EnterGameScene;
	}

	private void OnDisable()
	{
		_playableDirector.stopped -= EnterGameScene;
	}

	public void EnterGameScene(PlayableDirector director)
	{
        //Managers.Scene.LoadScene(Scene.LoadingScene);
    }

}
