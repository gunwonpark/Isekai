using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI_Loading : MonoBehaviour
{
    [Tooltip("FadeTest")]
    [SerializeField] private float _fadeTime = 2f;
    [SerializeField] private float _waitTimeAfterFade = 1f;
    [SerializeField] private float _waitTimeBeforeFade = 1.0f;

    [SerializeField] private Image _progressBar;
    [SerializeField] private Image _worldImage;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private Image _glitchPanel;

    [SerializeField] private TMP_Text _tipText;
    [SerializeField] private TMP_Text _worldText;
    [SerializeField] private TMP_Text _warningText;

    private LoadingSceneData _loadingSceneData;

    private IEnumerator Start()
    {
        _worldText.text = "";
        _tipText.text = "";

        _loadingSceneData = Managers.DB.GetLoadingSceneData(Managers.World.CurrentWorldType);
        
       
        if(_loadingSceneData.worldType == WorldType.Gang)
        {
            yield return StartCoroutine(GangrilLoadingSequence());
        }
        else if (_loadingSceneData.worldType == WorldType.Pelmanus)
        {
            yield return StartCoroutine(PelmanusLoadingSequence());
        }
        else
        {
            yield return StartCoroutine(LoadingSequence());
        }

        Managers.Scene.LoadScene(Scene.GameScene);
    }

    private IEnumerator LoadingSequence()
    {
        _tipText.text = $"{_loadingSceneData.tip}";
        _worldText.text = $"[{_loadingSceneData.name}]";

        yield return StartCoroutine(_progressBar.CoFillImage(0.8f, 2));
        yield return StartCoroutine(_progressBar.CoFillImage(1f, 2));

        yield return StartCoroutine(_fadeImage.CoFadeOut(_fadeTime));
    }

    private IEnumerator PelmanusLoadingSequence()
    {
        _glitchPanel.gameObject.SetActive(true);
        _worldText.text = _loadingSceneData.name.GetNRandomMaskedText(3);
        _worldText.text = $"[{_worldText.text}]";
        _tipText.text = _loadingSceneData.tip.GetRandomMaskedText();

        yield return StartCoroutine(_fadeImage.CoFadeIn(_fadeTime));
        // 게이지가 0 인상태에서 화면 노이즈는 4초간 지속된다
        // 이때 월드 이름의 일정 부분을 모자이크처리하고 팁 부분도 모자이크로 만들어 준다 
        yield return new WaitForSeconds(4f);

        // 4초뒤 노이즈가 끝나고 검은화면이 된다
        // 검은화면에서 대사를 빨간색으로 출력하고 2초간 대기한다
        _glitchPanel.gameObject.SetActive(false);
        _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 1f);
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(_warningText.CoTypeingEffect(_loadingSceneData.tip, 0.5f));
        yield return new WaitForSeconds(2f);

      
    }

   

    private IEnumerator GangrilLoadingSequence()
    {
        // 로딩바 2초동안 50% 상승
        yield return StartCoroutine(_progressBar.CoFillImage(0.5f, 3f));
        yield return new WaitForSeconds(1f);
        // 텍스트 타이핑 효과
        yield return StartCoroutine(_tipText.CoTypeingEffect(_loadingSceneData.tip, 0.3f));
        // 로딩바 남은부분 100%까지 1초 상승
        yield return StartCoroutine(_progressBar.CoFillImage(1f, 1f));
        // 팁 텍스트 3번 깜빡이기
        yield return StartCoroutine(_tipText.BlinkTipText(3, 0.2f));
        // 노이즈 효과 + 씬전환
        yield return StartCoroutine(NoiseEffect());

    }

    private IEnumerator NoiseEffect()
    {
        yield return new WaitForSeconds(2f);

    }
}
