using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EndingScene : UI_Scene
{
	[SerializeField] private TMP_Text _newsText;       // 뉴스 대사 텍스트
	[SerializeField] private TMP_Text _titleText;      // 제목 텍스트

	public override void Init()
	{
		base.Init();

		_newsText.text = "";       // 뉴스 텍스트 초기화
		_titleText.text = "";      // 제목 텍스트 초기화

		StartCoroutine(PlayEndingSequence());
	}

	private IEnumerator PlayEndingSequence()
	{
		// 뉴스 대사 타이핑 효과
		string newsDialogue = "속보입니다.\n최근 '이세계'에 빠져 '이 세계'의 자신을 잃어가는 사례가 급증하고 있다는 보고입니다.\n전문가들은 이러한 현상을 두고 '이세계 증후군'이라는 이름을 붙였습니다.";
		yield return StartCoroutine(TypeEffect(_newsText, newsDialogue, 0.05f));

		// 대사 출력 완료 후 2초 대기
		yield return new WaitForSeconds(2f);
		_newsText.gameObject.SetActive(false);

		// 제목 텍스트 페이드 인
		_titleText.text = "이세계 증후군";
		yield return StartCoroutine(FadeText(_titleText, 0f, 1f, 2f));

		// 메인 화면으로 전환
		Managers.Scene.LoadScene(Scene.GameScene); // Main Title Scene으로 전환
	}

	// 텍스트 타이핑 효과
	private IEnumerator TypeEffect(TMP_Text textComponent, string content, float typingSpeed)
	{
		foreach (char c in content)
		{
			textComponent.text += c;
			yield return new WaitForSeconds(typingSpeed);
		}
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
