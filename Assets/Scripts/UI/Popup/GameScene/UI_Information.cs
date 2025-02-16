using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.Timeline;

public class UI_Information : UI_Popup
{
    [SerializeField] private TextMeshProUGUI _yesText;
    [SerializeField] private TextMeshProUGUI _noText;

    public event Action onYesEvent;
    public override void Init()
    {
        base.Init();

        Color _initColor = _yesText.color;

        UI_EventHandler _yesTextEvent = _yesText.GetComponent<UI_EventHandler>();
        _yesTextEvent.OnPointerEnterHandler += (PointerEventData data) => { _yesText.color = Color.red; };
        _yesTextEvent.OnPointerExitHandler += (PointerEventData data) => { _yesText.color = _initColor; };
        _yesTextEvent.OnPointerUpHandler += (PointerEventData data) => 
        {
            //현실세계로 이동
            onYesEvent?.Invoke();
            //Managers.Scene.LoadScene(Scene.RealGameScene);
            Managers.UI.ClosePopupUI(this); 
        };

        UI_EventHandler _noTextEvent = _noText.GetComponent<UI_EventHandler>();
        _noTextEvent.OnPointerEnterHandler += (PointerEventData data) => { _noText.color = Color.red; };
        _noTextEvent.OnPointerExitHandler += (PointerEventData data) => { _noText.color = _initColor; };
        _noTextEvent.OnPointerUpHandler += (PointerEventData data) => { Managers.UI.ClosePopupUI(this); };
    }
}
