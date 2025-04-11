using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GangrilNotice : UI_Base
{
    [SerializeField] private Toggle _checkToggle;
    [SerializeField] private TMP_Text _noticeText;
    [SerializeField] private TMP_Text _warningText;
    [SerializeField] private Transform _noticeImage;
    [SerializeField] private Image _fadeImage;

    private int _index;

    //Notice창의 크기는 0.7, 1.0, 1.3 순으로 되어있다 
    public void Init(int index)
    {
        _noticeImage.gameObject.SetActive(false);
        StartCoroutine(ShowNoticeImage(index));
        
    }

    private IEnumerator ShowNoticeImage(int index)
    {
        yield return new WaitForSeconds(0.3f);

        _noticeImage.gameObject.SetActive(true);
        _index = index;

        switch (index)
        {
            case 1:
                SetNoticeProperties(0.7f, 50f);
                break;
            case 2:
                SetNoticeProperties(1.0f, 55f);
                break;
            case 3:
                SetNoticeProperties(1.3f, 60f);
                break;
            default:
                break;
        }

        _checkToggle.onValueChanged.AddListener(OnCheckToggleIsOn);

    }

    private void SetNoticeProperties(float scale, float fontSize)
    {
        _noticeImage.localScale = new Vector3(scale, scale, 1f);
        _noticeText.fontSize = fontSize;
    }

    // 3번까지 Notice창이 생성된다 3번 인덱스 에서는 게임씬으로 넘어간다.
    private void OnCheckToggleIsOn(bool isOn)
    {
        if (!isOn) return;


        if (_index == 3)
        {
            StartCoroutine(EnterGameScene());
        }
        else
        {
            // 다음 Notice창으로 넘어간다.
            var ui = Managers.UI.MakeSubItem<UI_GangrilNotice>();
            ui.Init(_index + 1);
            Managers.Resource.Destroy(gameObject);
        }
    }

    private IEnumerator EnterGameScene()
    {
        yield return StartCoroutine(_fadeImage.CoFadeOut(1f));
        Managers.Scene.LoadScene(Scene.GameScene);
    }

    public override void Init()
    {
    }
}
