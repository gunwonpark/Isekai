using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EndingScene : UI_Scene
{
	[SerializeField] private TMP_Text _newsText;       // 뉴스 대사 텍스트
	[SerializeField] private TMP_Text _titleText;      // 제목 텍스트

	[SerializeField] private AudioSource _effect;
	public override void Init()
	{
		base.Init();

		_newsText.text = "";       // 뉴스 텍스트 초기화
		_titleText.text = "";      // 제목 텍스트 초기화

        StartCoroutine(PlayEndingSequence());
	}

	private IEnumerator PlayEndingSequence()
	{
		yield return new WaitForSeconds(3f);
        _effect.Play();
        // 뉴스 대사 타이핑 효과
        string newsDialogue = "속보입니다.\n최근 '이세계'에 빠져 '이 세계'의 자신을 잃어가는 사례가 급증하고 있다는 보고입니다.\n전문가들은 이러한 현상을 두고 <color=red><b>'이세계 증후군'</b></color>이라는 이름을 붙였습니다.";
		yield return StartCoroutine(TypeEffect(_newsText, newsDialogue, 0.05f));

		// 대사 출력 완료 후 2초 대기
		yield return new WaitForSeconds(2f);
		_newsText.gameObject.SetActive(false);

		// 제목 텍스트 페이드 인
		_titleText.text = "이세계 증후군";
		yield return StartCoroutine(FadeText(_titleText, 0f, 1f, 2f));

		// 메인 화면으로 전환
		Managers.Scene.LoadScene(Scene.TitleScene); // Main Title Scene으로 전환
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
                _effect.Stop();
                yield return new WaitForSeconds(1.0f); // 줄바꿈 대기
                yield return StartCoroutine(FadeText(textComponent, 1f, 0f, 1f));

                textComponent.text = ""; // 텍스트 초기화
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1f);
                _effect.Play();
            }
            else
            {
                textComponent.text += c;
            }

            stringIndex++;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.5f); // 효과 마무리 시간
        _effect.Stop();
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
