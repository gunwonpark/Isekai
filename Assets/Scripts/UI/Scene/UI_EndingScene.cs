using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class UI_EndingScene : UI_Scene
{
	[SerializeField] private TMP_Text _newsText;       // 뉴스 대사 텍스트
	[SerializeField] private TMP_Text _finalText;      // 검은화면 텍스트
    [SerializeField] private GameObject _bubbleImage;  // 말풍선 이미지
    [SerializeField] private Image _fadeImage;

    [SerializeField] private VideoPlayer _endingVideoPlayer; // 마지막 글자 글리지 효과

    private EndingSceneData _sceneData;
	public override void Init()
	{
		base.Init();
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;

        _sceneData = Managers.DB.GetEndingSceneData();

        _newsText.text = "";       // 뉴스 텍스트 초기화
		_finalText.text = "";      // 제목 텍스트 초기화
        _bubbleImage.SetActive(false); // 말풍선 이미지 비활성화

        StartCoroutine(PlayEndingSequence());
	}

	private IEnumerator PlayEndingSequence()
	{
		yield return new WaitForSeconds(0.5f);
        _bubbleImage.SetActive(true); // 말풍선 이미지 활성화
        
        _newsText.text = _sceneData.newsDialog[0];
        ResizeBubbleImage(_sceneData.newsDialog[0]); // 말풍선 이미지 크기 조정

        yield return WaitForSecondsCache.Get(1.5f); // 1초 대기
        // 뉴스 텍스트 출력
        for (int i = 1; i < _sceneData.newsDialog.Count; i++)
        {
            ResizeBubbleImage(_sceneData.newsDialog[i]);
            yield return StartCoroutine(TypeEffect(_newsText, _sceneData.newsDialog[i], 0.1f));
        }

        // 화면 fadeOut
        yield return StartCoroutine(_fadeImage.CoFadeOut(1f));
        
        yield return WaitForSecondsCache.Get(1f); // 1초 대기

        // 검은 화면상의 텍스트 출력
        foreach (var finalDialogue in _sceneData.finalDialog)
        {
            yield return StartCoroutine(TypeEffect(_finalText, finalDialogue, 0.1f));
        }

        _finalText.text = "";

        yield return WaitForSecondsCache.Get(2f);

        // 비디오 효과로 글리치 효과
        _endingVideoPlayer.gameObject.SetActive(true); // 비디오 플레이어 활성화
        yield return WaitForSecondsCache.Get(3f); // 0.5초 대기

        // 메인 화면으로 전환
        //Managers.Scene.LoadScene(Scene.TitleScene); // Main Title Scene으로 전환
    }

    // 말풍선 이미지를 targetText의 크기에 맞게 조정한다
    private void ResizeBubbleImage(string targetText)
    {
        RectTransform bubbleRectTransform = _bubbleImage.GetComponent<RectTransform>();
        // 텍스트 크기를 계산한다
        Vector2 textSize = _newsText.GetPreferredValues(targetText);
        // 말풍선 이미지의 크기를 조정한다
        bubbleRectTransform.sizeDelta = new Vector2(textSize.x + 60, textSize.y + 30); // 여백 추가
        // 텍스트 위치 살짝 아래로 조정
        _newsText.rectTransform.anchoredPosition = new Vector2(0, -5f);
    }

    // 텍스트 타이핑 효과
    private IEnumerator TypeEffect(TMP_Text textComponent, string content, float typingSpeed)
    {
        textComponent.text = ""; // 초기화

        int stringIndex = 0;
        while (stringIndex < content.Length)
        {
            char c = content[stringIndex];

            if (c == '<') // Rich Text 태그 시작
            {
                int closeIndex = content.IndexOf('>', stringIndex);
                if (closeIndex == -1) // 태그가 정상적으로 닫히지 않음
                {
                    textComponent.text += c;
                }
                else
                {
                    textComponent.text += content.Substring(stringIndex, closeIndex - stringIndex + 1);
                    stringIndex = closeIndex; // 태그 끝까지 건너뛰기
                }
            }
            else if (c == '\n') // 줄바꿈 처리
            {
                yield return new WaitForSeconds(1.0f); // 줄바꿈 대기
                yield return StartCoroutine(FadeText(textComponent, 1f, 0f, 1f));

                textComponent.text = ""; // 텍스트 초기화
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1f);
            }
            else
            {
                textComponent.text += c;
            }

            stringIndex++;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.5f); // 효과 마무리 시간
    }

    // 텍스트 페이드 효과
    private IEnumerator FadeText(TMP_Text textComponent, float startAlpha, float endAlpha, float duration)
	{
		float time = 0f;
		Color color = textComponent.color;
		color.a = startAlpha;
		textComponent.color = color;

		while (time < duration)
		{
			time += Time.deltaTime;
			float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
			color.a = alpha;
			textComponent.color = color;
			yield return null;
		}

		color.a = endAlpha;
		textComponent.color = color;
	}
}
