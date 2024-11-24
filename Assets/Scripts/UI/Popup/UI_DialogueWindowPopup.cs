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

	private void Success()
	{
		if (countdownCoroutine != null)
			StopCoroutine(countdownCoroutine);

		onSuccess?.Invoke(happinessIncrease);
		ClosePopupUI();
	}

	private void Fail()
	{
		onFail?.Invoke(happinessDecrease);
		ClosePopupUI();
	}
}
