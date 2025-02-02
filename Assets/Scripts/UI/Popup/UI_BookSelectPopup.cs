using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BookSelectPopup : UI_Popup
{
	[SerializeField] private Image _selectImage1;
	[SerializeField] private Image _selectImage2;
	[SerializeField] private TMP_Text _selectText1;
	[SerializeField] private TMP_Text _selectText2;

	public override void Init()
	{
		base.Init();
		_selectImage1.gameObject.BindEvent(OnClickOpenBook);
		_selectImage2.gameObject.BindEvent(OnClickdecision);

	}

	public void ClosePopup()
	{
		Managers.UI.ClosePopupUI(this);
	}

	private void OnClickOpenBook(PointerEventData eventData)
	{
		Managers.UI.ClosePopupUI(this);
		Managers.UI.ShowPopupUI<UI_BookPopup>();
	}

	private void OnClickdecision(PointerEventData eventData)
	{
		Managers.UI.ClosePopupUI(this);
		Managers.UI.ShowPopupUI<UI_NoticePopup>();
	}
}
