using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteClickHandler : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _mouseRenderer;

	private bool _isClicked = false;
	
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

    private IEnumerator FingerTwinkleCoroutine()
    {
        while (true)
        {
            _mouseRenderer.enabled = !_mouseRenderer.enabled;

            yield return new WaitForSeconds(0.8f);
        }
    }
    public void StartBlink()
    {
        StartCoroutine(FingerTwinkleCoroutine());
    }

    public void StopBlink()
    {
        StopAllCoroutines();
        _mouseRenderer.enabled = false;
    }

    public void SetCanClicked()
	{
        _isClicked = false;
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
