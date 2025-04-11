using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class Sub_Toggle : UI_Base
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Sprite _onImage;
    [SerializeField] private Sprite _offImage;

    public int index;
    public Toggle Toggle => _toggle;
    public TMP_Text Text => _text;
    public void Init(string text, int index)
    {
        this.index = index;
        _toggle = GetComponent<Toggle>();

        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool arg0)
    {
        if (arg0)
        {
            _toggle.image.sprite = _onImage;
        }
        else
        {
            _toggle.image.sprite = _offImage;
        }
    }

    public override void Init()
    {
    }
}
