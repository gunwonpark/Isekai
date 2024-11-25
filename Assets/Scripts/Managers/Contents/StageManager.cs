using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
	public int happinessThreshold = 100;
	public int happinessMin = 0;
	public int maxDialogueCount = 6; // 한 스테이지 내 최대 대화 수

	private int currentDialogueCount = 0;

	private string currentSceneName;

	void Start()
	{
		// 스테이지 시작 시 초기화
		currentDialogueCount = 0;
		currentSceneName = SceneManager.GetActiveScene().name;
	}

	void Update()
	{
		float happiness = Managers.Happy.Happiness;

		if (happiness >= happinessThreshold)
		{
			ClearStage();
		}
		else if (happiness <= happinessMin || currentDialogueCount >= maxDialogueCount)
		{
			FailStage();
		}
	}

	public void OnDialogueSpawned()
	{
		currentDialogueCount++;
	}

	private void ClearStage()
	{
		// 포탈 생성
		GameObject portal = Managers.Resource.Instantiate("Portal", Managers.UI.Root.transform);
		Portal portalScript = portal.GetComponent<Portal>();
		portalScript.targetSceneName = GetNextSceneName();

		// 클리어 팝업 표시
		//Managers.UI.ShowPopupUI<UI_ClearPopup>();
	}

	private string GetNextSceneName()
	{
		switch (currentSceneName)
		{
			case "stage01":
				return "stage02";
			case "stage02":
				return "FirstLibrary";
			case "stage03":
				return "stage04";
			case "stage04":
				return "FirstLibrary";
		}
		return null;
	}

	private void FailStage()
	{
		// 게임 오버 팝업 표시
		//Managers.UI.ShowPopupUI<UI_FailPopup>();
		// 게임 오버 처리 로직 추가
	}
}
