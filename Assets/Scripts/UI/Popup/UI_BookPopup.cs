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
	}

	public void OnClickNextButton()
	{
	}
}
