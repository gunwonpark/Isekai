using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : UI_Base
{
    public event Action OnKeyPressed;
    private KeyCode _keyCode;
    public void Init(KeyCode keyCode, Sprite sprite)
    {
        _keyCode = keyCode;
        this.GetComponent<Image>().sprite = sprite;
    }
    private void Update()
    {
        if (Input.GetKeyDown(_keyCode)){
            OnKeyPressed?.Invoke();
            Destroy(gameObject);
        }
    }

    public override void Init()
    {
           
    }
}
