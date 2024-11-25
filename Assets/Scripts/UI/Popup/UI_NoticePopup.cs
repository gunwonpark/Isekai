using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NoticePopup : UI_Popup
{
    [SerializeField] private Button _button;

	public override void Init()
	{
		base.Init();
		_button.onClick.AddListener(OnClickNextSceneButton);
	}

	private void OnClickNextSceneButton()
	{
		Managers.Scene.LoadScene(Scene.stage01);
	}
}
