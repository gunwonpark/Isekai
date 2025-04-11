using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{	
	/// <summary>
	/// Start에서 초기화 되는 함수
	/// </summary>
	public abstract void Init();

	private void Start()
	{
		Init();
	}

	/// <summary>
	/// 이미지 클릭,드래그, 마우스의 이벤트에 발생하는 이벤트 바인딩
	/// 현재는 Click과 Drag만 존재
	/// </summary>
	public static void BindEvent(GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
	{
		UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

		switch (type)
		{
			case UIEvent.Click:
				evt.OnClickHandler -= action;
				evt.OnClickHandler += action;
				break;
			case UIEvent.Drag:
				evt.OnDragHandler -= action;
				evt.OnDragHandler += action;
				break;
        }
	}
}
