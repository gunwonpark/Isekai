using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneEx : BaseScene
{
	[SerializeField] private MiniGameFactory _miniGameFactory;
	[SerializeField] private Transform _player;
	[SerializeField] private WorldType _worldType = WorldType.Gang;

	[SerializeField] private List<GameObject> _portalList;
	[SerializeField] private float _gameStartDelay = 1.0f;
    protected override void Init()
	{
		base.Init();

		//_worldType = Managers.World.CurrentWorldType;

        Debug.Log($"CurrentWorld is : {_worldType}");
		Debug.Log("GameSceneEx Init");

		SceneType = Scene.GameScene;

		//미니게임 공장 초기화
		_miniGameFactory.Init(_worldType);
        _miniGameFactory.OnGameEnd += GameOver;

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
        }
		else
		{
            // 뒷 배경이 어둡게 처리되고, 게임오버창이 떠오른다
            Managers.UI.ShowPopupUI<UI_GameOver>();
        }
	}

	public override void Clear()
	{

	}
}
