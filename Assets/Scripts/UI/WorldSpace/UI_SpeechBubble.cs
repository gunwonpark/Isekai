using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeechBubble : UI_Base
{
	enum GameObjects
	{
		KeyImage,
		TimeGauge
	}

	enum Texts
	{
		DialogueText
	}

	[Header("Settings")]
	public bool isUpsideDown = false;      // 이미지 상하 반전 여부

	[Header("References")]
	public Image KeyImage;
	public Image TimeGauge;

	private string requiredKey;            // 필요한 키 입력
	private float timeLimit;               // 제한 시간
	private System.Action OnSuccess;
	private System.Action OnFail;

	private float timer;
	private bool isSuccess = false;

	private string[] keys = { "Q", "W", "E", "R", "A", "S", "D", "F" }; // 기본 키 배열

	public override void Init()
	{
		
	}

	public void Configure(string dialogue, int keyCount, float limit, System.Action onSuccess, System.Action onFail)
	{
		Bind<GameObject>(typeof(GameObjects));
		Bind<TMP_Text>(typeof(Texts));

		// 대사 설정
		SetDialogueText(dialogue);

		// 키 입력 설정
		requiredKey = GenerateRandomKey(keyCount);
		SetKeyImage(requiredKey);

		// 제한 시간 설정
		timeLimit = limit;
		timer = timeLimit;

		// 성공 및 실패 콜백 설정
		OnSuccess = onSuccess;
		OnFail = onFail;

		// 이미지 상하 반전 처리
		if (isUpsideDown)
		{
			GetImage((int)GameObjects.KeyImage).rectTransform.localScale = new Vector3(1, -1, 1);
		}

		// 키 입력 이벤트 등록
		//Managers.Happy.OnHappinessChanged += UpdateTimeGauge;
	}

	private void Update()
	{
		if (isSuccess) return;

		timer -= Time.deltaTime;
		UpdateTimeGauge();

		if (timer <= 0f)
		{
			Fail();
		}
		else if (Input.GetKeyDown(requiredKey.ToLower()))
		{
			Success();
		}
	}

	private void UpdateTimeGauge()
	{
		if (TimeGauge != null)
		{
			float fillAmount = timer / timeLimit;
			TimeGauge.fillAmount = fillAmount;
		}
	}

	private string GenerateRandomKey(int keyCount)
	{
		List<string> selectedKeys = new List<string>();
		while (selectedKeys.Count < keyCount)
		{
			string key = keys[Random.Range(0, keys.Length)];
			if (!selectedKeys.Contains(key))
			{
				selectedKeys.Add(key);
			}
		}
		return selectedKeys[Random.Range(0, selectedKeys.Count)];
	}

	private void SetKeyImage(string key)
	{
		if (KeyImage != null)
		{
			Sprite keySprite = Resources.Load<Sprite>($"KeyImages/{key}");
			if (keySprite != null)
			{
				KeyImage.sprite = keySprite;
			}
			else
			{
				Debug.LogError($"Key sprite not found: KeyImages/{key}");
			}
		}
	}

	private void SetDialogueText(string dialogue)
	{
		TMP_Text dialogueText = Get<TMP_Text>((int)Texts.DialogueText);
		if (dialogueText != null)
		{
			dialogueText.text = dialogue;
		}
	}

	private void Success()
	{
		if (isSuccess) return;
		isSuccess = true;
		OnSuccess?.Invoke();
		Managers.Resource.Destroy(gameObject);
	}

	private void Fail()
	{
		if (isSuccess) return;
		isSuccess = true;
		OnFail?.Invoke();
		Managers.Resource.Destroy(gameObject);
	}

	private void OnDestroy()
	{
		//Managers.Happy.OnHappinessChanged -= UpdateTimeGauge;
	}
}
