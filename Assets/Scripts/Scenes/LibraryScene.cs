using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 도서관 씬 상의 책 관리
/// 도서관 씬에서의 타임라인 관리
/// 배경 관리
/// </summary>
public class LibraryScene : BaseScene
{
    // 책 관리
    [SerializeField] private GameObject _bookParent;
    [SerializeField] private GameObject[] _books;

    // 타임라인 관리
	[SerializeField] private PlayableDirector _startTimeLine;
    [SerializeField] private PlayableDirector _endTimeLine;
    public event Action onStartTimeLineEnd;
    public event Action onEndTimeLineEnd;

    // 배경 관리
    // [SerializeField] private GameObject _background;
    // [SerializeField] private Material[] _backgroundMaterials;

    // 이동 예정
    [SerializeField] private Volume _volume;
    [SerializeField] private ColorAdjustments _noticePopupVolume;

    protected override void Init()
	{
		base.Init();

		SceneType = Scene.LibraryScene;

        _volume.profile.TryGet(out _noticePopupVolume);

        // 도서관에서의 플레이어 이동속도 설정
		Managers.DB.SetPlayerData(
			new PlayerData  
			{
				moveSpeed = new List<float> { 1f, 1f, 1f }
			});

        foreach (var book in _books)
        {
            book.GetComponent<LibraryBook>().Init();
        }

        _startTimeLine.stopped += OnStartTimeLineEnd;
        _endTimeLine.stopped += OnEndTimeLineEnd;
    }

    #region TimeLineMethod
    // 책 전부 밝혀주기
    private void OnStartTimeLineEnd(PlayableDirector director)
    {
        onStartTimeLineEnd?.Invoke();

        // 특정 책 만 켜주기
        BookSwitch();
    }

    private void OnEndTimeLineEnd(PlayableDirector director)
    {
        onEndTimeLineEnd?.Invoke();
    }

    public void PlayEndTimeLine()
    {
        _endTimeLine.Play();
    }
    #endregion

    #region BookMethod
    public void DisableBookSelect()
    {
        _bookParent.SetActive(false);
    } 
    public void EnableBookSelect()
    {
        _bookParent.SetActive(true);
    }
    //현재월드에 해당하는 책만 상호작용이 가능하게 한다
    private void BookSwitch()
    {
        EnableBookSelect();

        WorldType currentWorldType = Managers.World.CurrentWorldType;

        int bookIndex = (int)currentWorldType;
        LibraryBook book = _books[bookIndex].GetComponent<LibraryBook>();
        book.StartFingerBlink();
        book.SetCanClicked();
    }
    #endregion

    public override void Clear()
	{
        // 플레이어 이동속도 초기화
		Managers.DB.ResetPlayerData();

        _startTimeLine.stopped -= OnStartTimeLineEnd;
        _endTimeLine.stopped -= OnEndTimeLineEnd;
    }


    /// <summary>
    /// Gangril 월드에서 noticepopup의 깜빡임 효과 주기
    /// </summary>
    private Coroutine _colorConversionCoroutine;
    public void ColorConversion(float blinkTime)
    {
        _colorConversionCoroutine =  StartCoroutine(CoColorConversion(blinkTime));
    }

    public void StopColorConversion()
    {
        if (_colorConversionCoroutine != null)
        {
            _noticePopupVolume.colorFilter.value = originColor;
            StopCoroutine(_colorConversionCoroutine);
            _colorConversionCoroutine = null;
        }
    }

    private Color originColor = new Color(1f, 1f, 1f);
    private IEnumerator CoColorConversion(float blinkTime)
    {

        Color targetColor = new Color(140/255f, 0f, 0f);
        Color originColor = _noticePopupVolume.colorFilter.value;
      
        _noticePopupVolume.colorFilter.value = targetColor;
        yield return WaitForSecondsCache.Get(blinkTime);
        _noticePopupVolume.colorFilter.value = originColor;
        yield return WaitForSecondsCache.Get(blinkTime);
    }
}
