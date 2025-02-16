using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : UI_Base
{
    public event Action OnKeyPressed;
    [SerializeField] protected KeyCode _keyCode;
    [SerializeField] protected Image _image;

    protected bool _canPressKey = false;

    public virtual float Width
    {
        get
        {
            return _image.rectTransform.sizeDelta.x;
        }
    }

    public void Init(KeyCode keyCode, Sprite sprite)
    {
        _keyCode = keyCode;
        _image.sprite = sprite;
        _canPressKey = false;
    }

    private void Update()
    {
        if (_canPressKey && Input.GetKeyDown(_keyCode)){
            OnkeyPressedEvent();
            Destroy(gameObject);
        }
    }

    public void EnableKeyPress()
    {
        _canPressKey = true;
    }

    public void DisableKeyPress()
    {
        _canPressKey = false;
    }

    protected void OnkeyPressedEvent()
    {
        OnKeyPressed?.Invoke();
    }

    public override void Init()
    {
           
    }
}
