using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_TitleScene : UI_Scene
{
	[Header("Text")]
	[SerializeField] private TMP_Text _dialogueText;       // 대사 텍스트

	[Header("Fade")]
	[SerializeField] private CanvasGroup _dialogueCanvasGroup;  // 페이드용
	[SerializeField] private float _fadeDuration = 1.0f;

	[Header("Input")]
	[SerializeField] private TMP_InputField _inputField;    // 유저 입력용 (장면2)
	[SerializeField] private CanvasGroup _inputCanvasGroup; // 장면2 UI용

	// 대사들
	private string dialogue1 = "이 세계에서 상처받은 모든 이들에게 묻습니다.";
	private string dialogue2 = "이 세계에서 벗어나 이세계로 향한다면 \n우리는 과연 행복해질 수 있을까요?";
	private string dialogue3 = "당신의 답변에 담긴 의미는 무엇일까요? \n그것을 찾기 위한 첫걸음이 시작됩니다.";

	public override void Init()
	{
		base.Init();
		_dialogueText.text = "";
		_dialogueCanvasGroup.alpha = 1.0f;
		_inputCanvasGroup.alpha = 0f;
		_inputField.characterLimit = 50;
		_inputField.onSubmit.AddListener(OnInputSubmit);

		StartCoroutine(PlayIntroSequence());
	}

	private IEnumerator PlayIntroSequence()
	{
		// 장면 1
		yield return StartCoroutine(TypeAndWait(_dialogueText, dialogue1, 0.075f));
		yield return StartCoroutine(FadeOutText());

		yield return StartCoroutine(TypeAndWait(_dialogueText, dialogue2, 0.075f));
		yield return StartCoroutine(FadeOutText());

		// 장면 2
		yield return StartCoroutine(ShowInputField());
		// 유저 입력 대기 (OnInputSubmit 발생 전까지 대기)
		while (_inputCanvasGroup.alpha > 0 && _inputField.gameObject.activeSelf)
		{
			yield return null;
		}

		// 장면 3
		// 유저 입력 완료 후 마지막 대사 출력
		_dialogueText.text = "";
		yield return StartCoroutine(TypeAndWait(_dialogueText, dialogue3, 0.075f));
		yield return StartCoroutine(FadeOutText());

		// 게임 씬으로 페이드인
		Managers.Scene.LoadScene(Scene.IntroScene);
	}

	private IEnumerator TypeAndWait(TMP_Text textComponent, string content, float typingSpeed)
	{
		textComponent.text = "";
		// 대사 타이핑
		yield return StartCoroutine(TypeEffect(textComponent, content, typingSpeed));
		// 타이핑 완료 후 3초 대기
		yield return new WaitForSeconds(4f);
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

	private IEnumerator FadeOutText()
	{
		float startAlpha = _dialogueCanvasGroup.alpha;
		float elapsed = 0f;
		while (elapsed < _fadeDuration)
		{
			elapsed += Time.deltaTime;
			_dialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / _fadeDuration);
			yield return null;
		}

		_dialogueCanvasGroup.alpha = 0f;
		// 텍스트 비율
		_dialogueText.text = "";
		// 페이드 아웃 후 잠시 대기
		yield return new WaitForSeconds(0.5f);

		// 다시 페이드 인 (다음 대사 위해)
		elapsed = 0f;
		while (elapsed < _fadeDuration)
		{
			elapsed += Time.deltaTime;
			_dialogueCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / _fadeDuration);
			yield return null;
		}

		_dialogueCanvasGroup.alpha = 1f;
	}

	private IEnumerator ShowInputField()
	{
		// 입력 필드 표시
		_inputField.text = "";
		_inputField.gameObject.SetActive(true);

		float elapsed = 0f;
		float startAlpha = _inputCanvasGroup.alpha;
		while (elapsed < _fadeDuration)
		{
			elapsed += Time.deltaTime;
			_inputCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, elapsed/ _fadeDuration);
			yield return null;
		}
		_inputCanvasGroup.alpha = 1f;

		_inputField.Select();
		_inputField.ActivateInputField();
	}

	private void OnInputSubmit(string userInput)
	{
		// 엔터 입력 시 입력 필드 페이드 아웃
		StartCoroutine(HideInputField());
	}

	private IEnumerator HideInputField()
	{
		float elapsed = 0f;
		float startAlpha = _inputCanvasGroup.alpha;
		while (elapsed < _fadeDuration)
		{
			elapsed += Time.deltaTime;
			_inputCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed/ _fadeDuration);
			yield return null;
		}
		_inputCanvasGroup.alpha = 0f;
		_inputField.gameObject.SetActive(false);

	}
}
