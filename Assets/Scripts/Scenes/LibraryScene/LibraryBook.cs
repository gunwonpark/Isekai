using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 책 hover시 이벤트
/// 책 클릭유도 이벤트 정의
/// </summary>
public class LibraryBook : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _mouseRenderer;
    [SerializeField] private BoxCollider2D _collider;
	[SerializeField] private bool _isClicked = false;           // 클릭되었는지 여부
    [SerializeField] private float _fingerBlinkSpeed = 0.8f;    // 손가락 깜박거림 대기시간

    // 초기 책 상태 설정
    public void Init()
    {
        _mouseRenderer.enabled = false;
    }

    public void StartFingerBlink()
    {
        StartCoroutine(CoFingerBlink());
    }

    public void StopFingerBlink()
    {
        StopAllCoroutines();
        _mouseRenderer.enabled = false;
    }

    // 책 클릭 초기화
    public void SetCanClicked()
	{
        _isClicked = false;
        _collider.enabled = true;
    }

    // 손가락 깜박거림
    private IEnumerator CoFingerBlink()
    {
        while (true)
        {
            _mouseRenderer.enabled = !_mouseRenderer.enabled;

            yield return WaitForSecondsCache.Get(_fingerBlinkSpeed);
        }
    }

    public void OnMouseDown()
    {
        if (!_isClicked)
        {
            var ui = Managers.UI.MakeWorldSpaceUI<UI_BookSelectWorldSpace>();
            ui.Init(this);

            StopFingerBlink();
            _isClicked = true;
        }
    }

    // 초기화
    private void Reset()
    {
        _collider = GetComponent<BoxCollider2D>();
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                _mouseRenderer = sr;
                break;
            }
        }
    }
}
