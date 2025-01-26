using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeKeyButton : TwoKeyButton
{
    [SerializeField] protected KeyCode _thirdKeyCode;
    [SerializeField] protected Image _thirdImage;

    public void Init(KeyCode keyCode, KeyCode secondKeyCode, KeyCode thirdKeyCode, Sprite sprite, Sprite sceondSprite, Sprite thirdSprite)
    {
        base.Init(keyCode, secondKeyCode, sprite, sceondSprite);
        _thirdKeyCode = thirdKeyCode;
        _thirdImage.sprite = thirdSprite;
    }

    private void Update()
    {
        if(_canPressKey && Input.GetKeyDown(_keyCode) && Input.GetKeyDown(_secondKeyCode) && Input.GetKeyDown(_thirdKeyCode))
        {
            OnkeyPressedEvent();
            Destroy(gameObject);
        }
    }

}
