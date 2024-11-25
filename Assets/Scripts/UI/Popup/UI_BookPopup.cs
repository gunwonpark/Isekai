using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BookPopup : UI_Popup
{
    [SerializeField] private Image _letterImage;
    [SerializeField] private TMP_Text _letterText;
	[SerializeField] private TMP_Text _TitleText;
    [SerializeField] private Button _nextButton;

	public override void Init()
	{
		base.Init();
		if (_nextButton == null)
		{
			Debug.LogError("_nextButton이 null입니다.");
			return;
		}
		_nextButton.onClick.RemoveAllListeners(); // 기존 이벤트 제거
		_nextButton.onClick.AddListener(OnClickNextButton);
		Debug.Log("Next 버튼 이벤트가 연결되었습니다.");
	}

	public void OnClickNextButton()
	{
		Managers.Resource.Instantiate("UI/Popup/UI_NoticePopup");
		Debug.Log("도대체");
		Destroy(gameObject);
	}
}
