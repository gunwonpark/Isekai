using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoKeyButton : KeyButton
{
    [SerializeField] protected KeyCode _secondKeyCode;
    [SerializeField] protected Image _secondImage;

    public override float Width
    {
        get
        {
            return base.Width + _secondImage.rectTransform.sizeDelta.x + 0.5f;
        }
    }

    public void Init(KeyCode keyCode, KeyCode secondKeyCode, Sprite sprite, Sprite sceondSprite)
    {
        base.Init(keyCode, sprite);
        _secondKeyCode = secondKeyCode;
        _secondImage.sprite = sceondSprite;
    }
    private void Update()
    {
        if (Input.GetKeyDown(_keyCode) && Input.GetKeyDown(_secondKeyCode))
        {
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
}

