using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpriteFactory
{
    private Dictionary<KeyCode, Sprite> _keySprites;

    public const string _keySpritePath = "Art/Keyboard/";

    public void Init()
    {
        MakeKeySprites();
    }

    public Sprite GetKeySprite(KeyCode keyCode)
    {
        if (_keySprites.ContainsKey(keyCode))
            return _keySprites[keyCode];
        else
            return null;
    }

    private void MakeKeySprites()
    {
        _keySprites = new Dictionary<KeyCode, Sprite>();
        for (int i = 0; i < 26; i++)
        {
            char key = (char)('A' + i);
            string path = _keySpritePath + key;
            Sprite sprite = Resources.Load<Sprite>(path);

            if(sprite != null)
                _keySprites.Add((KeyCode)(Enum.Parse(typeof(KeyCode), key.ToString())), sprite);
        }
    }
}
