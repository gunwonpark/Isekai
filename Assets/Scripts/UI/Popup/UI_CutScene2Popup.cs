using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CutScene2Popup : UI_Popup
{
	[SerializeField] private Image _letterImage;
	[SerializeField] private TMP_Text _letterText;
	[SerializeField] private TMP_Text _TitleText;
	[SerializeField] private Button _nextButton;

	public override void Init()
	{
		base.Init();
	}

	public void ClosePopup()
	{
		Managers.UI.ClosePopupUI(this);
	}
}
