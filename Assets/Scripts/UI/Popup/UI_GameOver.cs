using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameOver : UI_Popup
{
    [SerializeField] private TextMeshProUGUI _infoText;

    private bool _canPassOver = false;

    private void Awake()
    {
        StartCoroutine(ShowInfo());
    }

    private IEnumerator ShowInfo()
    {
        Color color = _infoText.color;
        color.a = 0f;

        while(color.a < 1f)
        {
            color.a += Time.deltaTime;
            _infoText.color = color;
            yield return null;
        }

        color.a = 1f;
        _infoText.color = color;
        _canPassOver = true;
    }

    private void Update()
    {
        if (!_canPassOver) return;

        // 아무 공간이나 클릭하면 자신의 씬 다시시작 이동
        if (Input.GetMouseButton(0))
        {
            Managers.Scene.LoadScene(Scene.GameScene);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
