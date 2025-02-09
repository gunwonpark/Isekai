using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BookSelectWorldSpace : UI_Base
{
	[SerializeField] private Image _selectImage1;
	[SerializeField] private Image _selectImage2;
	[SerializeField] private TMP_Text _selectText1;
	[SerializeField] private TMP_Text _selectText2;
	[SerializeField] private GameObject[] bookTransforms;

	public override void Init()
	{
		bookTransforms = GameObject.FindGameObjectsWithTag("Book");
		_selectImage1.gameObject.BindEvent(OnClickOpenBook);
		_selectImage2.gameObject.BindEvent(OnClickdecision);
		SetTransform();
	}

	private void OnClickOpenBook(PointerEventData eventData)
	{
		Destroy(gameObject);
		Managers.UI.ShowPopupUI<UI_BookPopup>();
	}

	private void OnClickdecision(PointerEventData eventData)
	{
		Destroy(gameObject);
		Managers.UI.ShowPopupUI<UI_NoticePopup>();
	}

	private void SetTransform()
	{
		WorldType currentWorldType = Managers.World.CurrentWorldType;

		switch (currentWorldType)
		{
			case WorldType.Vinter:
				transform.position = bookTransforms[0].transform.position + new Vector3(2f, 0f, 0f);
				break;
			case WorldType.Chaumm:
				transform.position = bookTransforms[1].transform.position + new Vector3(2f, 0f, 0f);
				break;
			case WorldType.Gang:
				transform.position = bookTransforms[2].transform.position + new Vector3(2f, 0f, 0f);
				break;
			case WorldType.Pelmanus:
				transform.position = bookTransforms[3].transform.position + new Vector3(2f, 0f, 0f);
				break;
		}
	}
}
