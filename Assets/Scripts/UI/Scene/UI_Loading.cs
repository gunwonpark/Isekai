using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    [SerializeField] private float _fadeTime = 2f;
    [SerializeField] private float _waitTimeAfterFade = 1f;
    [SerializeField] private float _waitTimeBeforeFade = 1f;

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
        _tipText.text = string.Empty;
        _worldText.text = string.Empty;
        _progressBar.fillAmount = 0f;

        _loadingSceneData = Managers.DB.GetLoadingSceneData(Managers.World.CurrentWorldType);

        yield return StartCoroutine(GetLoadingSequence());

        Managers.Scene.LoadScene(Scene.GameScene);
    }

    private IEnumerator GetLoadingSequence()
    {
        switch (_loadingSceneData.worldType)
        {
            case WorldType.Gang:
                yield return GangrilLoadingSequence();
                break;
            case WorldType.Pelmanus:
                yield return PelmanusLoadingSequence();
                break;
            default:
                yield return DefaultLoadingSequence();
                break;
        }
    }

    private IEnumerator DefaultLoadingSequence()
    {
        SetLoadingTexts();
        yield return FillProgressBar(0.8f, 2f);
        yield return FillProgressBar(1f, 2f);
        yield return StartCoroutine(_fadeImage.CoFadeIn(_fadeTime));
    }

    private IEnumerator PelmanusLoadingSequence()
    {
        _glitchPanel.gameObject.SetActive(true);
        _worldText.text = $"[{_loadingSceneData.name.GetNRandomMaskedText(3)}]";
        _tipText.text = _loadingSceneData.tip.GetRandomMaskedText();

        yield return StartCoroutine(_fadeImage.CoFadeOut(_fadeTime));
        yield return new WaitForSeconds(4f);

        _glitchPanel.gameObject.SetActive(false);
        _fadeImage.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(_warningText.CoTypingEffect(_loadingSceneData.tip, 0.5f));
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator GangrilLoadingSequence()
    {
        yield return FillProgressBar(0.5f, 3f);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(_tipText.CoTypingEffect(_loadingSceneData.tip, 0.3f));
        yield return FillProgressBar(1f, 1f);
        yield return StartCoroutine(_tipText.CoBlinkText(3, 0.2f));
        yield return new WaitForSeconds(2f);  // NoiseEffect() Á¦°Å
    }

    private IEnumerator FillProgressBar(float target, float duration)
    {
        yield return StartCoroutine(_progressBar.CoFillImage(target, duration));
    }

    private void SetLoadingTexts()
    {
        _tipText.text = _loadingSceneData.tip;
        _worldText.text = $"[{_loadingSceneData.name}]";
    }
}