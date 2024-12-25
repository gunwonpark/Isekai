using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BlamePopup : UI_Popup
{
	[SerializeField] private Image[] _talkBalloonImages;
	[SerializeField] private TMP_Text[] _dialogueTexts;
	[SerializeField] private PlayerController _playerController;

	private string[] _dialogues = {
		"너만 뒤쳐지고 있는 거 아니야?",
		"네 친구는 벌써 승진했다더라",
		"왜 그 정도도 못하니?",
		"다들 집 샀다던데, 너는 계획 있어?",
		"너 진짜 혼자만 다르게 산다."
	};

	private float _glitchDuration = 1f; // 글리치 효과 지속 시간

	public override void Init()
	{
		base.Init();
		// 모든 대화 풍선 이미지를 비활성화하고 텍스트를 초기화합니다.
		for (int i = 0; i < _talkBalloonImages.Length; i++)
		{
			_talkBalloonImages[i].gameObject.SetActive(false);
			_dialogueTexts[i].text = "";
		}

		// 대사 출력 코루틴 시작
		StartCoroutine(ShowDialogues());
		_playerController = FindObjectOfType<PlayerController>();
	}

	private IEnumerator ShowDialogues()
	{
		for (int i = 0; i < _dialogues.Length; i++)
		{
			// 해당 대화 풍선 이미지를 활성화합니다.
			_talkBalloonImages[i].gameObject.SetActive(true);

			// 타자 치는 효과로 대사 출력
			yield return StartCoroutine(TypeEffect(_dialogueTexts[i], _dialogues[i]));

			// 행복 게이지 감소
			Managers.Happy.ChangeHappiness(-10f);

			// 카메라 흔들림 효과 적용 (더 역동적으로)
			ShakeCamera(duration: 0.5f, magnitude: 0.3f);

			// 다음 대사까지 1초 대기
			yield return new WaitForSeconds(1f);
		}

		// 모든 대사가 끝난 후 2초 대기
		yield return new WaitForSeconds(2f);

		// 글리치 효과와 페이드 아웃을 순차적으로 실행
		StartCoroutine(GlitchEffect());
		yield return StartCoroutine(FadeOutDialogues());

		// 모든 처리가 끝난 후 팝업 닫기 또는 다음 행동 수행
		ClosePopupUI();
	}

	private IEnumerator TypeEffect(TMP_Text textComponent, string dialogue)
	{
		textComponent.text = "";
		foreach (char c in dialogue)
		{
			textComponent.text += c;
			yield return new WaitForSeconds(0.05f); // 타자 치는 속도 조절 가능
		}
	}

	private void ShakeCamera(float duration = 0.5f, float magnitude = 0.3f)
	{
		StartCoroutine(ShakeCoroutine(duration, magnitude));
	}

	private IEnumerator ShakeCoroutine(float duration, float magnitude)
	{
		Transform cameraTransform = Camera.main.transform;
		Vector3 originalPos = cameraTransform.localPosition;
		float elapsed = 0.0f;

		while (elapsed < duration)
		{
			float x = Mathf.Sin(Time.time * 50f) * magnitude * (1f - (elapsed / duration));
			float y = Mathf.Cos(Time.time * 60f) * magnitude * (1f - (elapsed / duration));

			cameraTransform.localPosition = originalPos + new Vector3(x, y, 0f);

			elapsed += Time.deltaTime;

			yield return null;
		}

		cameraTransform.localPosition = originalPos;
	}

	private IEnumerator GlitchEffect()
	{
		float elapsed = 0f;
		float interval = 0.05f; // 글리치 효과의 업데이트 간격
		while (elapsed < _glitchDuration)
		{
			// 각 대화 풍선 이미지와 텍스트에 글리치 효과 적용
			for (int i = 0; i < _talkBalloonImages.Length; i++)
			{
				if (_talkBalloonImages[i].gameObject.activeSelf)
				{
					// 위치 변형
					RectTransform rectTransform = _talkBalloonImages[i].rectTransform;
					Vector3 originalPosition = rectTransform.anchoredPosition;
					rectTransform.anchoredPosition = originalPosition + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);

					// 색상 변형
					_talkBalloonImages[i].color = new Color(Random.value, Random.value, Random.value);

					// 텍스트 변형
					_dialogueTexts[i].color = new Color(Random.value, Random.value, Random.value);
					_dialogueTexts[i].fontSize = Random.Range(20, 30);
				}
			}

			yield return new WaitForSeconds(interval);

			// 변형된 값 복원
			for (int i = 0; i < _talkBalloonImages.Length; i++)
			{
				if (_talkBalloonImages[i].gameObject.activeSelf)
				{
					// 위치 복원
					RectTransform rectTransform = _talkBalloonImages[i].rectTransform;
					rectTransform.anchoredPosition3D = rectTransform.anchoredPosition3D;

					// 색상 복원
					_talkBalloonImages[i].color = Color.white;
					_dialogueTexts[i].color = Color.black;
					_dialogueTexts[i].fontSize = 24; // 원래 폰트 크기로 복원
				}
			}

			elapsed += interval;
		}
	}

	// 페이드 아웃 코루틴 추가
	private IEnumerator FadeOutDialogues()
	{
		float duration = 1f;
		float elapsed = 0f;

		// 각 대화 풍선 이미지와 텍스트의 초기 색상 저장
		Color[] balloonColors = new Color[_talkBalloonImages.Length];
		Color[] textColors = new Color[_dialogueTexts.Length];

		for (int i = 0; i < _talkBalloonImages.Length; i++)
		{
			balloonColors[i] = _talkBalloonImages[i].color;
			textColors[i] = _dialogueTexts[i].color;
		}

		while (elapsed < duration)
		{
			float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

			// 알파 값 감소 적용
			for (int i = 0; i < _talkBalloonImages.Length; i++)
			{
				if (_talkBalloonImages[i].gameObject.activeSelf)
				{
					Color balloonColor = balloonColors[i];
					balloonColor.a = alpha;
					_talkBalloonImages[i].color = balloonColor;

					Color textColor = textColors[i];
					textColor.a = alpha;
					_dialogueTexts[i].color = textColor;
				}
			}

			elapsed += Time.deltaTime;
			yield return null;
		}

		// 최종적으로 알파 값을 0으로 설정하고 오브젝트 비활성화
		for (int i = 0; i < _talkBalloonImages.Length; i++)
		{
			if (_talkBalloonImages[i].gameObject.activeSelf)
			{
				Color balloonColor = _talkBalloonImages[i].color;
				balloonColor.a = 0f;
				_talkBalloonImages[i].color = balloonColor;

				Color textColor = _dialogueTexts[i].color;
				textColor.a = 0f;
				_dialogueTexts[i].color = textColor;

				_talkBalloonImages[i].gameObject.SetActive(false);
			}
		}
		GameObject go = Managers.Resource.Instantiate("Cutscene/cutscene1 Variant");
		yield return new WaitForSeconds(8.5f);
		Managers.Resource.Destroy(go);
		Managers.UI.ShowPopupUI<UI_LetterPopup>();
		yield return new WaitForSeconds(10.5f);
		Debug.Log("Hello");
		Managers.Resource.Instantiate("Cutscene/after cutscene Variant");
		yield return new WaitForSeconds(6f);
		_playerController.isMoving = true;
	}
}
