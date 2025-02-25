using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : UI_Scene
{
    [Tooltip("FadeTest")]
    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private float waitTimeAfterFade = 1f;

    [SerializeField] private Image _fadeImage;
    [SerializeField] private Button _startButton;
    public override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        _startButton.onClick.AddListener(ShowTitleUI);
    }

    private void ShowTitleUI()
    {
        StartCoroutine(CoShowSceneUI());
    }

    IEnumerator CoShowSceneUI()
    {
        yield return _fadeImage.CoFadeOut(fadeTime, waitTimeAfterFade);
        Managers.UI.ShowSceneUI<UI_TitleScene>();

        this.gameObject.SetActive(false);
    }

    
}
