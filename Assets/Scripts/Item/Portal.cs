using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fadeTime = 1f;

    private UI_Information _information;

    public event System.Action onEnterEvent;

    private Scene _nextScene = Scene.RealGameScene;

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(_spriteRenderer.CoFadeIn(_fadeTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_information != null || _information.gameObject.activeInHierarchy == false) return;

        if (collision.CompareTag("Player"))
        {
            _information = Managers.UI.ShowPopupUI<UI_Information>();
            _information.onYesEvent += OnEnterEvent;
        }
    }

    private void OnEnterEvent()
    {
        onEnterEvent?.Invoke();
        Managers.Scene.LoadScene(_nextScene);
    }

    public void SetPortalPosition(Scene targetScene)
    {
        _nextScene = targetScene;
    }
}
