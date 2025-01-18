using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene2AnimationEvents : MonoBehaviour
{
	private UI_CutScene2Popup cutScene2Popup;
	private PlayerController _playerController;

	private void Start()
	{
		_playerController = FindObjectOfType<PlayerController>();
		cutScene2Popup = GetComponentInParent<UI_CutScene2Popup>();
	}

	public void OnCutScene2AnimationEnd()
	{
		cutScene2Popup.ClosePopupUI();
		Managers.Resource.Instantiate("CutScene/after cutscene Variant");
	}
}
