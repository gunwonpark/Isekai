using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteClickHandler : MonoBehaviour
{
	private bool _isClicked;

	private void OnEnable()
	{
		_isClicked = false;
	}

	private void OnMouseDown()
	{
		if (!_isClicked)
		{
			Managers.UI.MakeWorldSpaceUI<UI_BookSelectWorldSpace>();
			_isClicked = true;
		}		
	}
}
