using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bubble : UI_Base
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private Image _bubbleImage;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] private float _widthPadding = 0.1f;
    [SerializeField] private float _heightPadding = 0.1f;
    
    [SerializeField] private float _textSize = 0.3f;

    private int _score;
    public string testText = "평범하기 짝이 없으면서 어딜 나서려고 하는 거야?";

    public event Action OnCollisionEvent;
    
    public void Init(string text, int score)
    {
        _text.text = text;
        _text.fontSize = _textSize;

        _score = score;

        float length = _text.preferredWidth;
        float hight = _text.preferredHeight;

        Vector2 preferredSize = new Vector2(length + _widthPadding, hight + _heightPadding);
        _bubbleImage.rectTransform.sizeDelta = preferredSize;

        _boxCollider.size = preferredSize - new Vector2(0.1f, 0.1f);
    }

    private void Update()
    {
        Vector2 pos = transform.position;
        pos.y -= Time.deltaTime;
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Managers.Happy.ChangeHappiness(_score);
            OnCollisionEvent?.Invoke();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            OnCollisionEvent?.Invoke();
            Destroy(gameObject);
        }
    }
    
    public override void Init()
    {
       
    }
}
