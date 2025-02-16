using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fadeDuration = 1f;

    private UI_Information _information;

    public event System.Action onYesEvent;

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(CoFadeIn());
    }

    private IEnumerator CoFadeIn()
    {
        Color color = _spriteRenderer.color;
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += _fadeDuration * Time.deltaTime;
            color.a = alpha;
            _spriteRenderer.color = color;
            yield return null;
        }

        color.a = 1f;
        _spriteRenderer.color = color;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_information != null) return;

        if (collision.CompareTag("Player"))
        {
            _information = Managers.UI.ShowPopupUI<UI_Information>();
            _information.onYesEvent += OnYesEvent;
        }
    }

    private void OnYesEvent()
    {
        onYesEvent?.Invoke();
    }
}
