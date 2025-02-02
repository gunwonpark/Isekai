using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RealGameInfo
{
    public string dialog;
    public int score;

    public RealGameInfo(string dialog, int score)
    {
        this.dialog = dialog;
        this.score = score;
    }
}

public class RealGameFactory : MonoBehaviour
{
    private RealWorldInfo _realWorldInfo;
    private int _gameIndex = 0;
    private int _gameCount = 0;

    [SerializeField] private float _makeGameDelay = 2.0f;

    [SerializeField] private Vector2 _spawnPos = new Vector2(6, 8f);

    private List<UI_Bubble> _bubbles = new List<UI_Bubble>();

    public event Action<bool> OnGameEnd;
    public void Init(RealWorldInfo realWorldInfo)
    {
        _realWorldInfo = realWorldInfo;
        _gameCount = _realWorldInfo.dialog.Count;

        StartCoroutine(COMakeRealGame());
    }

    public void MakeRealGame()
    {
        UI_Bubble ui_Bubble = Managers.UI.MakeSubItem<UI_Bubble>(this.transform);
        _bubbles.Add(ui_Bubble);
        Vector2 pos = new Vector2(UnityEngine.Random.Range(-_spawnPos.x, _spawnPos.x), _spawnPos.y);
        ui_Bubble.transform.position = pos;

        ui_Bubble.Init(_realWorldInfo.dialog[_gameIndex], _realWorldInfo.score[_gameIndex]);

        ui_Bubble.OnCollisionEvent += Ui_Bubble_OnCollisionEvent;

    }

    private void Ui_Bubble_OnCollisionEvent()
    {
        _gameCount--;

        if (Managers.Happy.Happiness <= 0)
        {
            for(int i = 0; i < _bubbles.Count; i++)
            {
                UI_Bubble bubble = _bubbles[i];
                if(bubble != null)
                {
                    Destroy(bubble.gameObject);
                }
            }
            StopAllCoroutines();
            OnGameEnd.Invoke(false);
        }
        else if(_gameCount == 0)
        {
            StopAllCoroutines();
            OnGameEnd.Invoke(true);
        }
    }

    IEnumerator COMakeRealGame()
    {
        for(_gameIndex = 0; _gameIndex < _realWorldInfo.dialog.Count; _gameIndex++) { 
            MakeRealGame();
            yield return new WaitForSeconds(_makeGameDelay);
        }
    }
}
