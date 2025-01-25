using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEx : BaseScene
{
	[SerializeField] private MiniGameFactory _miniGameFactory;
	protected override void Init()
	{
		base.Init();

		SceneType = Scene.GameScene;

		//미니게임 공장 초기화
		_miniGameFactory.Init(WorldType.Vinter);

		//배경음악 재생
        Managers.Sound.Play("anotherWorldBgm", Sound.Bgm);
	}

	//게임 종료시
	public void GameOver(bool isWin)
	{
		if (isWin)
		{
			// 현실세계로 이동하는 포탈이 생성된다
		}
		else
		{
			// 뒷 배경이 어둡게 처리되고, 게임오버창이 떠오른다
		}
	}

	public override void Clear()
	{

	}
}
