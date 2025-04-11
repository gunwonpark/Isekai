using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 책 hover시 나타나는 UI
/// 책에 대한 내용을 확인할지, 책 세상속으로 진입할지 결정
/// </summary>
public class UI_BookSelectWorldSpace : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _selectImage1;
	[SerializeField] private Image _selectImage2;
	[SerializeField] private TMP_Text _selectText1;
	[SerializeField] private TMP_Text _selectText2;

	private Color defaultColor1;
	private Color defaultColor2;
	
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
		_book = book;
    }

    private void OnClickOpenBook(PointerEventData eventData)
	{
		var ui = Managers.UI.ShowPopupUI<UI_BookPopup>();
		ui.Init(_book);
        Managers.Resource.Destroy(gameObject);
	}

	private void OnClickdecision(PointerEventData eventData)
	{
		var ui = NoticePopupFactory.CreatePopup(Managers.World.CurrentWorldType);
        ui.Init(_book);
        Managers.Resource.Destroy(gameObject);
	}

	private void SetTransform()
	{
		WorldType currentWorldType = Managers.World.CurrentWorldType;

        transform.position = _book.transform.position + new Vector3(2f, -0.4f, 0f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerEnter == _selectImage1.gameObject)
		{
			_selectImage1.color = Color.yellow;
		}
		else if (eventData.pointerEnter == _selectImage2.gameObject)
		{
			_selectImage2.color = Color.yellow;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerEnter == _selectImage1.gameObject)
		{
			_selectImage1.color = defaultColor1;
		}
		else if (eventData.pointerEnter == _selectImage2.gameObject)
		{
			_selectImage2.color = defaultColor2;
		}
	}
}
