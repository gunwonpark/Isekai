using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class GameSceneEx : BaseScene
{
	// 이를 이용하는 것은 추후에 ObjectManager를 사용하며 player를 관리하게 만드는 것이 좋겠다.
	public static Transform player;

	[SerializeField] private MiniGameFactory _miniGameFactory;
	[SerializeField] private Transform _player;
	
	[SerializeField] private Image _fadeImage;

    [SerializeField] private WorldType _worldType = WorldType.Gang;

	//[SerializeField] private List<GameObject> _portalList;
	[SerializeField] private float _gameStartDelay = 1.0f;
    protected override void Init()
	{
		base.Init();

        _worldType = Managers.World.CurrentWorldType;
        GameSceneEx.player = _player;

        Debug.Log($"CurrentWorld is : {_worldType}");
		Debug.Log("GameSceneEx Init");

		SceneType = Scene.GameScene;

		//미니게임 공장 초기화
		_miniGameFactory.Init(_worldType);
        _miniGameFactory.OnGameEnd += GameOver;

		GameObject background = Managers.Resource.Instantiate($"Background/{_worldType.ToString()}World");
        //배경음악 재생
        Managers.Sound.Play("anotherWorldBgm", Sound.Bgm);
	}

	//게임 종료시
	public void GameOver(bool isWin)
	{
		if (isWin)
		{
			// 현실세계로 이동하는 포탈이 생성된다
			GameObject go = Managers.Resource.Instantiate("Item/Portal");
			Vector3 newPosition = _player.position + new Vector3(8f, 0, 0);
            newPosition.y = -2.5f;
            go.transform.position = newPosition;

			go.GetComponent<Portal>().onYesEvent += ClearEvent;
        }
		else
		{
            // 뒷 배경이 어둡게 처리되고, 게임오버창이 떠오른다
            Managers.UI.ShowPopupUI<UI_GameOver>();
        }
	}

	private void ClearEvent()
	{
        StartCoroutine(CoFadeOut());
    }

    private IEnumerator CoFadeOut()
    {
        float fadeOutTime = 3.0f;
        float currentTime = 0.0f;
        Color color = _fadeImage.color;

        while (currentTime < fadeOutTime)
        {
            currentTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, currentTime / fadeOutTime);
            _fadeImage.color = color;
            yield return null;
        }

        Managers.Scene.LoadScene(Scene.RealGameScene);

        yield return null;
    }


    public override void Clear()
	{

	}
}
