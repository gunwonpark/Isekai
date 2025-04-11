using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;
using UnityEngine.UI;

/// <summary>
/// 1. 현재 월드 생성
/// 2. 게임 시작시 이벤트
///     음악재생
///     페이드인
/// 3. 게임 종료시 이벤트
///     포탈생성
/// 4. postprocessing을 통한 효과 적용  
/// 5. 엔딩 씬으로 이동시 이벤트
/// </summary>
public class GameSceneEx : BaseScene
{

	public Transform player;

    // 미니게임을 제조한다
	[SerializeField] private MiniGameFactory _miniGameFactory;

    [Header("UI")]
	[SerializeField] private Image _fadeImage;

    [Header("페이드 시간")]
    [SerializeField] private float _fadeTime = 3f;
    [SerializeField] private float _waitTimeAfterFade = 0f;
    [SerializeField] private float _waitTimeBeforeFade = 0f;

    [Header("포탈 위치")]
    [SerializeField] private float _potalSpawnOffsetX = 8f;
    [SerializeField] private float _potalSpawnOffsetY = -2.5f;

    [Header("포스트 프로세싱")]
    [SerializeField] private Volume _volume;

    private WorldType _worldType;
    protected override void Init()
	{
		base.Init();
		SceneType = Scene.GameScene;

        _worldType = Managers.World.CurrentWorldType;

		Managers.Resource.Instantiate($"Background/{_worldType.ToString()}World");
        Managers.Happy.ChangeHappiness(20f);

        StartCoroutine(GameStart());
	}

    private IEnumerator GameStart()
    {
        yield return StartCoroutine(_fadeImage.CoFadeIn(_fadeTime, waitAfter : _waitTimeAfterFade));

        // 배경음악 재생
        Managers.Sound.Play("anotherWorldBgm", Sound.Bgm);

        // 미니게임 생성
        _miniGameFactory.Init();
        _miniGameFactory.OnGameEnd += GameOver;
    }

    /// <summary>
    /// 승리시 :
    ///     현실세계로 이동하는 포탈 생성
    /// 패배시 :
    ///     게임오버 UI 생성
    /// </summary>
    public void GameOver(bool isWin)
	{
		if (isWin)
        { 
            if(Managers.World.CurrentWorldType == WorldType.Pelmanus)
            {
                StartCoroutine(EnterEndingScene());
                return;
            }

			GameObject go = Managers.Resource.Instantiate("Item/Portal");
			Vector3 newPosition = player.position + new Vector3(_potalSpawnOffsetX, _potalSpawnOffsetY, 0);
            go.transform.position = newPosition;

            Portal portal = go.GetComponent<Portal>();
            portal.SetPortalPosition(Scene.RealGameScene);
            portal.onEnterEvent += ClearEvent;
        }
		else
		{
            Managers.UI.ShowPopupUI<UI_GameOver>();
        }
	}

    /// <summary>
    /// 강도에 따른 postprocessing 효과
    /// </summary>
    /// <param name="strength">postprocessing 강도 조절</param>
    // Film Grain, Vignette, Chromatic Aberration조절
    public void SetPostProcessing(int strength)
    {
        
        switch (strength) 
        {
            case 4:
                AdjustVolume(0.05f, 0.1f, 0.3f);
                break;
            case 5:
                AdjustVolume(0.1f, 0.3f, 0.5f);
                break;
            case 6:
                AdjustVolume(0.15f, 0.4f, 0.7f);
                break;
        }
    }

    private void AdjustVolume(float filmIntensity, float vignetteIntensity, float chromaticAberrationIntensity)
    {
        _volume.gameObject.SetActive(true);
        if (_volume.profile.TryGet(out FilmGrain filmGrain))
        {
            filmGrain.intensity.Override(filmIntensity);
        }
        if (_volume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.Override(vignetteIntensity);
        }
        if (_volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.intensity.Override(chromaticAberrationIntensity);
        }
    }

    private IEnumerator EnterEndingScene()
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 1);

        yield return WaitForSecondsCache.Get(2f);

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
