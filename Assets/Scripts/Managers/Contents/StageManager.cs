using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	public int happinessThreshold = 100;
	public int happinessMin = 0;
	public int maxDialogueCount = 6; // 한 스테이지 내 최대 대화 수

	private int currentDialogueCount = 0;

	void Start()
	{
		// 스테이지 시작 시 초기화
		currentDialogueCount = 0;
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
		GameObject portal = Managers.Resource.Instantiate("Prefabs/Portal", Managers.UI.Root.transform);
		Portal portalScript = portal.GetComponent<Portal>();
		portalScript.targetSceneName = GetNextSceneName();

		// 클리어 팝업 표시
		//Managers.UI.ShowPopupUI<UI_ClearPopup>();
	}

	private string GetNextSceneName()
	{
		// 현재 씬의 다음 씬 이름을 반환하는 로직 구현
		// 예시로 "NextStage" 반환
		return "NextStage";
	}

	private void FailStage()
	{
		// 게임 오버 팝업 표시
		//Managers.UI.ShowPopupUI<UI_FailPopup>();
		// 게임 오버 처리 로직 추가
	}
}
