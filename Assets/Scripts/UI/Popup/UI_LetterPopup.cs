using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LetterPopup : UI_Popup
{
	[SerializeField] private TMP_Text _letterText;
	[SerializeField] private Image _letterImage;

	private string _dialogues = "이 세계에 지친 당신.\n이 세계에 싫증을 느낀 당신.\n이 세계를 리셋하고\n 이세계로 떠나고픈 당신\n\n그런 당신에게 추천합니다.\n\n'이세계 도서관'에서 행복하고 아름다운 이세계를 체험하고 행복을 되찾아보세요.";

	public override void Init()
	{
		base.Init();
		_letterText.text = "";
		StartCoroutine(typingEffectCo(_dialogues));
	}

	private IEnumerator typingEffectCo(string dialogue)
	{
		foreach (char c in dialogue)
		{
			_letterText.text += c;
			yield return new WaitForSeconds(0.05f); // 타자 치는 속도 조절 가능
		}

		yield return new WaitForSeconds(5.0f);
		Managers.UI.ShowPopupUI<UI_CutScene2Popup>();

		gameObject.SetActive(false);
	}
}
