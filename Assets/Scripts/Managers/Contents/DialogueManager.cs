using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	// 스테이지별 대사 목록을 추가로 관리할 수 있습니다.
	private Dictionary<string, string[]> stageDialogues = new Dictionary<string, string[]>();

	void Start()
	{
		stageDialogues.Add("Stage01", new string[] {
		"역시 명문 가문의 품격을 갖춘 완벽한 인물이셔",
		"공작님의 재능은 신이 내리신 선물이야.",
		"공작님의 외모는 신이 내리신 예술 작품 같아.",
		"공작님을 칭송하는 것 만으로도 영광이야!!",
		"그의 존재 자체가 이 국가에 큰 축복이지.",
		"모든 영애가 공작님의 외모만 보면 눈물을 흘린다지?"
	});

		// Stage3 대사 추가
		stageDialogues.Add("Stage02", new string[] {
		"평범하기 짝이 없다면서 어딜 나서려고 하는 거야?",
		"넌 진짜 존재감이라고는 1도 없다",
		"외모도 능력도 특별할 것 없는 사람에게 뭘 기대하겠어.",
		"누가 너한테 관심이나 있을 줄 알아?",
		"넌 애초에 부족한 사람이야",
		"너는 대체 뭘 잘하니?"
	});

		// Stage3 대사 추가
		stageDialogues.Add("Stage03", new string[] {
		"너의 지혜와 재치는 그 누구도 따라올 수 없을거야",
		"유머와 영리함이 너의 가장 큰 매력이야.",
		"너는 사람들 사이에서도 두각을 드러내는 특별한 존재야.",
		"함께 있으면 즐거워!!",
		"너의 긍정적 에너지가 나에게 많은 힘이 돼",
		"너는 참 믿음직 스러워",
		"너의 적응력은 정말 대단해",
	});
		// Stage3 대사 추가
		stageDialogues.Add("Stage04", new string[] {
		"그 직업으로는 돈 많이 못 벌지 않아?",
		"진짜 아는 게 뭐야",
		"너네 가족들은 너한테 기대 많이 하셨을텐데...",
		"또 실패했어?",
		"이것도 못해?",
		"다 잘되라고 하는 소리야.",
		"시간 낭비야 그거",
		"언제쯤 남들처럼 살거야?",
		"너한테 너무 큰 기대였구나.",
		"언제까지 힘들래?",
	});
		stageDialogues.Add("Stage05", new string[] {
		"너의 존재만으로도 세상이 축복받은 것 같아",
		"그는 희망을 불어넣는 빛과 같은 존재야!!",
		"너의 곁에는 항상 평화와 경외심이 가득해.",
		"정말 성스러움이 느껴져",
		"그의 손길은 기적을 일으켜",
		"바라보기만 해도 경이로워",
		"그의 존재는 인간에게 있어 가장 큰 선물이야",
		"당신을 바라보기만 해도 행복해",
		"나도 너처럼 행복해지고 싶어",
	});
		// 다른 스테이지 추가 가능
		StartCoroutine(SpawnDialogues());
	}

	IEnumerator SpawnDialogues()
	{
		while (true)
		{
			yield return new WaitForSeconds(2f); // 대화창 생성 간격

			string currentScene = Managers.Scene.CurrentScene.SceneType.ToString();
			if (!stageDialogues.ContainsKey(currentScene))
				continue;

			string[] currentDialogues = stageDialogues[currentScene];
			int index = Random.Range(0, currentDialogues.Length);
			string dialogue = currentDialogues[index];

			// 스테이지별 키 개수 설정 (예: 4개)
			int keyCount = GetKeyCountForStage(currentScene);

			// 필요한 키 리스트 생성
			List<string> requiredKeys = GenerateRandomKeys(keyCount);

			// 팝업 생성
			UI_DialogueWindowPopup dialoguePopup = Managers.UI.ShowPopupUI<UI_DialogueWindowPopup>();

			// 팝업 설정
			dialoguePopup.Setup(
				dialogue,
				requiredKeys,
				OnDialogueSuccess,
				OnDialogueFail,
				limitTime: GetDialogueTimeLimit(currentScene),
				increase: GetHappinessIncrease(currentScene),
				decrease: GetHappinessDecrease(currentScene)
			);

			// 현재 대화 수 관리
			Managers.Stage.OnDialogueSpawned();
		}
	}

	private List<string> GenerateRandomKeys(int keyCount)
	{
		string[] keys = { "Q", "W", "E", "R", "A", "S", "D", "F" };
		List<string> selectedKeys = new List<string>();

		while (selectedKeys.Count < keyCount)
		{
			string key = keys[Random.Range(0, keys.Length)];
			if (!selectedKeys.Contains(key))
			{
				selectedKeys.Add(key);
			}
		}

		return selectedKeys;
	}

	private int GetKeyCountForStage(string sceneName)
	{
		// 스테이지별 요구되는 키 개수를 반환
		switch (sceneName)
		{
			case "Stage01":
				return 5; // 예: 4개의 키 요구
			case "Stage02":
				return 5;
			case "Stage03":
				return 5;
			case "Stage04":
				return 5;
			case "Stage05":
				return 5;
			default:
				return 4;
		}
	}

	private float GetDialogueTimeLimit(string sceneName)
	{
		// 스테이지별 제한 시간 설정
		switch (sceneName)
		{
			case "Stage01":
				return 3f;
			case "Stage02":
				return 1.5f;
			case "Stage03":
				return 2f;
			case "Stage04":
				return 1f;
			case "Stage05":
				return 2f;
			default:
				return 3f;
		}
	}

	private int GetHappinessIncrease(string sceneName)
	{
		// 스테이지별 행복도 증가량 설정
		switch (sceneName)
		{
			case "Stage01":
				return 40;
			case "Stage02":
				return 5;
			case "Stage03":
				return 20;
			case "Stage04":
				return 5;
			case "Stage05":
				return 1;
			default:
				return 40;
		}
	}

	private int GetHappinessDecrease(string sceneName)
	{
		// 스테이지별 행복도 감소량 설정
		switch (sceneName)
		{
			case "Stage01":
				return 5;
			case "Stage02":
				return 20;
			case "Stage03":
				return 5;
			case "Stage04":
				return 10;
			case "Stage05":
				return 5;
			default:
				return 10;
		}
	}

	private void OnDialogueSuccess(int happinessIncrease)
	{
		Managers.Happy.ChangeHappiness(happinessIncrease);
	}

	private void OnDialogueFail(int happinessDecrease)
	{
		Managers.Happy.ChangeHappiness(happinessDecrease);
	}
}
