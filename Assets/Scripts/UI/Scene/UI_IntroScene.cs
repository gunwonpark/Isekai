using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_IntroScene : UI_Scene
{
	[SerializeField] private Image _fadeImage;

	public override void Init()
	{
		base.Init();

		FadeIn(2f);
	}

	// 페이드 인 메서드
	public void FadeIn(float duration)
	{
		StartCoroutine(Fade(1f, 0f, duration));
	}

	// 페이드 아웃 메서드
	public void FadeOut(float duration)
	{
		StartCoroutine(Fade(0f, 1f, duration));
	}

	// 페이드 코루틴
	private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
	{
		float time = 0f;
		Color color = _fadeImage.color;
		color.a = startAlpha;
		_fadeImage.color = color;

		while (time < duration)
		{
			time += Time.deltaTime;
			float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
			color.a = alpha;
			_fadeImage.color = color;
			yield return null;
		}

		color.a = endAlpha;
		_fadeImage.color = color;

		yield return new WaitForSeconds(3f);
		Managers.UI.ShowPopupUI<UI_BlamePopup>();
	}
}
