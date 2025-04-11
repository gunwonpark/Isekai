using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LibraryBook : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _mouseRenderer;

	[SerializeField] private bool _isClicked = false;
	
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            _mouseRenderer = child.GetComponent<SpriteRenderer>();
            if (_mouseRenderer != null)
            {
                break;
            }
        }
        _mouseRenderer.enabled = false;
    }

    

    public void StartFingerBlink()
    {
        StartCoroutine(FingerBlink());
    }

    public void StopBlink()
    {
        StopAllCoroutines();
        _mouseRenderer.enabled = false;
    }

    // 책 클릭이 가능하게 해준다
    public void SetCanClicked()
	{
        _isClicked = false;
    }

    // 손가락 깜박거림 표시
    private IEnumerator FingerBlink()
    {
        while (true)
        {
            _mouseRenderer.enabled = !_mouseRenderer.enabled;

            yield return new WaitForSeconds(0.8f);
        }
    }

    public void OnMouseDown()
    {
        if (!_isClicked)
        {
            var ui = Managers.UI.MakeWorldSpaceUI<UI_BookSelectWorldSpace>();
            ui.Init(this);

            StopBlink();
            _isClicked = true;
        }
    }
}
