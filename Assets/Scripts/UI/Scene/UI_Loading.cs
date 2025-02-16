using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private Image _worldImage;
    [SerializeField] private Image _fadeImage;

    [SerializeField] private TMP_Text _tipText;
    [SerializeField] private TMP_Text _worldText;
    private void Start()
    {
        StartCoroutine(CoLoadScene());
    }

   
    IEnumerator CoLoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(Scene.EndingScene.ToString());
        op.allowSceneActivation = false;
        float timer = 0.0f;
        float timeToWait = 4.0f;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (_progressBar.fillAmount < 0.9f)
            {
                _progressBar.fillAmount = Mathf.Lerp(0, op.progress, timer / timeToWait);
                if (_progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                _progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer / timeToWait);
                if (_progressBar.fillAmount == 1.0f)
                {
                    yield return StartCoroutine(FadeIn());
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0.0f;
        float timeToWait = 2.0f;
        Color color = _fadeImage.color;

        while (timer < timeToWait)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / timeToWait);
            _fadeImage.color = color;
            yield return null;
        }
    }
}
