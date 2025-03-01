using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

/// <summary>
/// 게임 시작과 게임 종료 게임 정보를 저장하고 있는 클래스
/// </summary>
public class GameSceneEx : BaseScene
{
    [SerializeField] private WorldType _worldType = WorldType.Pelmanus;

	// 이를 이용하는 것은 추후에 ObjectManager를 사용하며 player를 관리하게 만드는 것이 좋겠다.
	public static Transform player;

    // 미니게임을 제조한다
	[SerializeField] private MiniGameFactory _miniGameFactory;

	[SerializeField] private Transform _player;

    [Header("UI")]
	[SerializeField] private Image _fadeImage;

    [Header("페이드 시간")]
    [SerializeField] private float _fadeTime = 3f;
    [SerializeField] private float _waitTimeAfterFade = 0f;
    [SerializeField] private float _waitTimeBeforeFade = 0f;

    [Header("포탈 위치")]
    [SerializeField] private float _potalSpawnOffsetX = 8f;
    [SerializeField] private float _potalSpawnOffsetY = -2.5f;
    protected override void Init()
	{
		base.Init();
		SceneType = Scene.GameScene;

        _worldType = Managers.World.CurrentWorldType;
        GameSceneEx.player = _player;

		Managers.Resource.Instantiate($"Background/{_worldType.ToString()}World");
        Managers.Happy.ChangeHappiness(20f);

        StartCoroutine(GameStart());
	}

    private IEnumerator GameStart()
    {
        yield return StartCoroutine(_fadeImage.CoFadeIn(_fadeTime, _waitTimeAfterFade));

        //배경음악 재생
        Managers.Sound.Play("anotherWorldBgm", Sound.Bgm);

        _miniGameFactory.Init();
        _miniGameFactory.OnGameEnd += GameOver;
    }

    //게임 종료시
    public void GameOver(bool isWin)
	{
		if (isWin)
        { 
            if(Managers.World.CurrentWorldType == WorldType.Pelmanus)
            {
                StartCoroutine(EnterEndingScene());
                return;
            }

			// 현실세계로 이동하는 포탈이 생성된다
			GameObject go = Managers.Resource.Instantiate("Item/Portal");
			Vector3 newPosition = _player.position + new Vector3(_potalSpawnOffsetX, _potalSpawnOffsetY, 0);
            go.transform.position = newPosition;

            Portal portal = go.GetComponent<Portal>();
            portal.SetPortalPosition(Scene.RealGameScene);
            portal.onEnterEvent += ClearEvent;
        }
		else
		{
            // 뒷 배경이 어둡게 처리되고, 게임오버창이 떠오른다
            Managers.UI.ShowPopupUI<UI_GameOver>();
        }
	}

    private IEnumerator EnterEndingScene()
    {
        yield return StartCoroutine(_fadeImage.CoFadeOut(_fadeTime, _waitTimeAfterFade, _waitTimeBeforeFade));

        Managers.Scene.LoadScene(Scene.EndingScene);
    }

    private void ClearEvent()
	{
        StartCoroutine(_fadeImage.CoFadeOut(3f));
    }



    public override void Clear()
	{

	}
}
