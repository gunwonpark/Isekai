using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityPopupSpawner
{
    private float minSpawnTime = 0.1f;
    private float startSpawnTime = 1.0f;
    private float acceleration = 0.95f;

    private float popupWidth, popupHeight;
    private float currentSpawnTime;
    private float spawnX, spawnY;
    private float screenWidth, screenHeight;
    private float leftTopX, leftTopY;

    private UI_NoticePopup _originPopup;
    private Coroutine _spawnCoroutine;

    public InfinityPopupSpawner(UI_NoticePopup originPopup, Vector3 startPosition)
    {
        _originPopup = originPopup;
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        spawnX = startPosition.x;
        spawnY = startPosition.y;

        currentSpawnTime = startSpawnTime;

        popupWidth = _originPopup.GetComponent<RectTransform>().rect.width;
        popupHeight = _originPopup.GetComponent<RectTransform>().rect.height;

        CalculateFirstPosition();
    }

    public void StartSpawning()
    {
        if (_spawnCoroutine != null) return;
        _spawnCoroutine = _originPopup.StartCoroutine(SpawnPopups());
    }

    private void CalculateFirstPosition()
    {
        leftTopX = -screenWidth / 2 + (screenWidth / 2) % 50;
        leftTopY = screenHeight / 2 - (screenHeight / 2) % 50;

        leftTopX = Mathf.Max(leftTopX, -screenWidth / 2 + popupWidth / 2);
        leftTopY = Mathf.Min(leftTopY, screenHeight / 2 - popupHeight / 2);
    }

    private IEnumerator SpawnPopups()
    {
        while (true)
        {
            SpawnPopup();

            yield return new WaitForSeconds(currentSpawnTime);

            if (currentSpawnTime > minSpawnTime)
            {
                currentSpawnTime *= acceleration;
                if (currentSpawnTime < minSpawnTime)
                    currentSpawnTime = minSpawnTime;
            }
        }
    }

    private void SpawnPopup()
    {
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
        UI_NoticePopup popup = Managers.UI.ShowPopupUI<UI_NoticePopup>();
        //popup.Init(_originPopup.PopupIndex + 1, false, spawnPosition, this);

        spawnX += 50;
        spawnY -= 50;

        if (spawnX + popupWidth / 2 > screenWidth / 2 || spawnY - popupHeight / 2 < -screenHeight / 2)
        {
            spawnX = leftTopX;
            spawnY = leftTopY;
        }
    }
}
