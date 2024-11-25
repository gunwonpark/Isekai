using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DialogueWindowPopup : UI_Popup
{
	[SerializeField] private TMP_Text dialogueText;
	[SerializeField] private Image[] keyImages;
	[SerializeField] private Slider timeSlider;

	private List<string> requiredKeys;
	private float timeLimit;
	private Coroutine countdownCoroutine;

	private System.Action<int> onSuccess;
	private System.Action<int> onFail;

	private int happinessIncrease;
	private int happinessDecrease;

	public override void Init()
	{
		base.Init();
		// 추가 초기화가 필요하다면 여기서 수행
	}

	public void Setup(string dialogue, List<string> keys, System.Action<int> successCallback, System.Action<int> failCallback, float limitTime = 3f, int increase = 40, int decrease = 5)
	{
		dialogueText.text = dialogue;
		requiredKeys = keys;
		onSuccess = successCallback;
		onFail = failCallback;

		timeLimit = limitTime;
		timeSlider.maxValue = timeLimit;
		timeSlider.value = timeLimit;

		happinessIncrease = increase;
		happinessDecrease = decrease;

		// 모든 keyImages를 비활성화
		foreach (var image in keyImages)
		{
			image.gameObject.SetActive(false);
		}

		// 필요한 키의 개수만큼 keyImages를 활성화하고 이미지 설정
		for (int i = 0; i < requiredKeys.Count && i < keyImages.Length; i++)
		{
			keyImages[i].gameObject.SetActive(true);
			Sprite keySprite = Resources.Load<Sprite>($"KeyImages/{requiredKeys[i]}");
			if (keySprite != null)
			{
				keyImages[i].sprite = keySprite;
			}
			else
			{
				Debug.LogError($"키 이미지 로드 실패: KeyImages/{requiredKeys[i]}");
			}
		}

		SetRandomPosition();

		countdownCoroutine = StartCoroutine(Countdown());
	}

	private IEnumerator Countdown() // 비제네릭 IEnumerator 사용
	{
		float timer = timeLimit;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			timeSlider.value = timer;
			yield return null;
		}

		// 시간 초과 시 실패 처리
		Fail();
	}

	private void Update()
	{
		foreach (var key in requiredKeys)
		{
			if (Input.GetKeyDown(key.ToLower()))
			{
				Success();
				break;
			}
		}
	}

	/// <summary>
	/// 화면의 오른쪽 또는 왼쪽에 랜덤한 위치로 팝업 배치
	/// </summary>
	private void SetRandomPosition()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		if (rectTransform == null)
		{
			Debug.LogError("RectTransform이 필요합니다.");
			return;
		}

		// 화면 크기 가져오기
		Canvas canvas = GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			Debug.LogError("Canvas가 필요합니다.");
			return;
		}

		// anchor를 설정하여 캔버스 전체를 기준으로 잡음
		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);

		// Canvas의 화면 너비와 높이
		RectTransform canvasRect = canvas.GetComponent<RectTransform>();
		float canvasWidth = canvasRect.rect.width;
		float canvasHeight = canvasRect.rect.height;

		// 랜덤 X 위치 (왼쪽 또는 오른쪽)
		float randomX = Random.value < 0.5f ? -canvasWidth / 4f : canvasWidth / 4f;

		// Y 위치는 중앙에 가까운 랜덤 값
		float randomY = Random.Range(-canvasHeight / 4f, canvasHeight / 4f);

		// 팝업의 위치 설정
		rectTransform.anchoredPosition = new Vector2(randomX, randomY);

		Debug.Log($"Canvas Width: {canvasRect.rect.width}, Canvas Height: {canvasRect.rect.height}");
		Debug.Log($"Random Position: X={randomX}, Y={randomY}");
		Debug.Log($"Final Anchored Position: {rectTransform.anchoredPosition}");
	}

	private void Success()
	{
		if (countdownCoroutine != null)
			StopCoroutine(countdownCoroutine);

		onSuccess?.Invoke(happinessIncrease);
		Managers.UI.ClosePopupUI();
	}

	private void Fail()
	{
		onFail?.Invoke(happinessDecrease);
		Managers.UI.ClosePopupUI();
	}
}
