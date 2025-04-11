using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BookSelectWorldSpace : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _selectImage1;
	[SerializeField] private Image _selectImage2;
	[SerializeField] private TMP_Text _selectText1;
	[SerializeField] private TMP_Text _selectText2;
	[SerializeField] private GameObject bookTransform;

	private Color defaultColor1;
	private Color defaultColor2;

	//private Vector3 defalultScale1;
	//private Vector3 defalultScale2;

	private LibraryBook _book;
    public override void Init()
	{
		_selectImage1.gameObject.BindEvent(OnClickOpenBook);
		_selectImage2.gameObject.BindEvent(OnClickdecision);

		defaultColor1 = _selectImage1.color;
		defaultColor2 = _selectImage2.color;

		SetTransform();
	}

	public void Init(LibraryBook book)
	{
		bookTransform = book.gameObject;
		_book = book;
    }

    private void OnClickOpenBook(PointerEventData eventData)
	{
        Managers.Resource.Destroy(gameObject);
		var ui = Managers.UI.ShowPopupUI<UI_BookPopup>();
		ui.Init(_book);
	}

	private void OnClickdecision(PointerEventData eventData)
	{
        Managers.Resource.Destroy(gameObject);
		var ui = Managers.UI.ShowPopupUI<UI_NoticePopup>();
		ui.Init(_book);
	}

	private void SetTransform()
	{
		WorldType currentWorldType = Managers.World.CurrentWorldType;

		switch (currentWorldType)
		{
			case WorldType.Vinter:
				transform.position = bookTransform.transform.position + new Vector3(2f, -0.4f, 0f);
				break;
			case WorldType.Chaumm:
				transform.position = bookTransform.transform.position + new Vector3(2f, -0.4f, 0f);
				break;
			case WorldType.Gang:
				transform.position = bookTransform.transform.position + new Vector3(2f, -0.4f, 0f);
				break;
			case WorldType.Pelmanus:
				transform.position = bookTransform.transform.position + new Vector3(2f, -0.4f, 0f);
				break;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerEnter == _selectImage1.gameObject)
		{
			Debug.Log("자세히보기");
			_selectImage1.color = Color.yellow;
			//_selectImage1.transform.localScale = Vector3.one * 1.1f;

		}
		else if (eventData.pointerEnter == _selectImage2.gameObject)
		{
			Debug.Log("이 책으로 결정하기");
			_selectImage2.color = Color.yellow;
			//_selectImage2.transform.localScale = Vector3.one * 1.1f; 
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerEnter == _selectImage1.gameObject)
		{
			_selectImage1.color = defaultColor1;
			//_selectImage1.transform.localScale = defalultScale1;
		}
		else if (eventData.pointerEnter == _selectImage2.gameObject)
		{
			_selectImage2.color = defaultColor2;
			//_selectImage2.transform.localScale = defalultScale2;
		}
	}
}
