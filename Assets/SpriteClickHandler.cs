using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteClickHandler : MonoBehaviour
{
	private void OnMouseDown()
	{
		Managers.UI.ShowPopupUI<UI_BookSelectPopup>();
	}
}
