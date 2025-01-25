using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fadeDuration = 1f;

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
        if (collision.CompareTag("Player"))
        {
            Managers.UI.ShowPopupUI<UI_Information>();
        }
    }
}
