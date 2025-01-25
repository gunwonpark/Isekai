using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Information : UI_Popup
{
    [SerializeField] private TextMeshProUGUI _yesText;
    [SerializeField] private TextMeshProUGUI _noText;

    public override void Init()
    {
        base.Init();

        Color _initColor = _yesText.color;

        UI_EventHandler _yesTextEvent = _yesText.GetComponent<UI_EventHandler>();
        _yesTextEvent.OnPointerEnterHandler += (PointerEventData data) => { Debug.Log("Point"); _yesText.color = Color.red; };
        _yesTextEvent.OnPointerExitHandler += (PointerEventData data) => { _yesText.color = _initColor; };
        _yesTextEvent.OnPointerUpHandler += (PointerEventData data) => 
        { 
            //현실세계로 이동
            Managers.Scene.LoadScene(Scene.TestScene);
            Managers.UI.ClosePopupUI(this); 
        };

        UI_EventHandler _noTextEvent = _noText.GetComponent<UI_EventHandler>();
        _noTextEvent.OnPointerEnterHandler += (PointerEventData data) => { _noText.color = Color.red; };
        _noTextEvent.OnPointerExitHandler += (PointerEventData data) => { _noText.color = _initColor; };
        _noTextEvent.OnPointerUpHandler += (PointerEventData data) => { Managers.UI.ClosePopupUI(this); };
    }
}
