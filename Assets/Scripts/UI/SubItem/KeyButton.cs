using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : UI_Base
{
    public event Action OnKeyPressed;
    public event Action OnKeyMissed;

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
        if (Input.GetKeyDown(_keyCode)){
            if (_canPressKey)
            {
                OnkeyPressedEvent();
                Destroy(gameObject);
            }
            else
            {
                OnKeyMissedEvent();
            }
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

    protected void OnKeyMissedEvent()
    {
        OnKeyMissed?.Invoke();
    }

    public override void Init()
    {
           
    }
}
