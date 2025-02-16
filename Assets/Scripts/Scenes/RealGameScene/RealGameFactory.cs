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
    [SerializeField] private int _gameIndex = 0;
    [SerializeField] private int _gameCount = 0;

    [SerializeField] private float _makeGameDelay = 1.0f;

    [SerializeField] private Transform _target;

    //[SerializeField] private Vector2 _spawnPos = new Vector2(6, 8f);
    
    private List<UI_Bubble> _bubbles = new List<UI_Bubble>();

    public event Action<bool> OnGameEnd;
    public void Init(RealWorldInfo realWorldInfo)
    {
        _realWorldInfo = realWorldInfo;
        _gameCount = _realWorldInfo.dialog.Count;

        StartCoroutine(CoMakeRealGame());
        //StartCoroutine(COMakeRealGame());
    }

    public void MakeRealGame()
    {
        UI_Bubble ui_Bubble = Managers.UI.MakeSubItem<UI_Bubble>(this.transform);
        _bubbles.Add(ui_Bubble);

        //Vector2 pos = new Vector2(UnityEngine.Random.Range(-_spawnPos.x, _spawnPos.x), _spawnPos.y);
        //ui_Bubble.transform.position = pos;

        int randomIndex = UnityEngine.Random.Range(0, 2);

        if(randomIndex == 0)
        {
            ui_Bubble.transform.position = new Vector2(_target.position.x, 8);
        }
        else
        {
            float x = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
            float y = 8f;

            ui_Bubble.transform.position = new Vector2(x, y);
        }

        ui_Bubble.Init(_realWorldInfo.dialog[_gameIndex], _realWorldInfo.score[_gameIndex]);

        ui_Bubble.OnCollisionEvent += Ui_Bubble_OnCollisionEvent;
        _gameIndex++;
    }

  

    private IEnumerator CoMakeRealGame()
    {
        yield return new WaitForSeconds(_makeGameDelay);
        MakeRealGame();
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
            return;
        }


        if(_gameCount == 0)
        {
            StopAllCoroutines();
            OnGameEnd.Invoke(true);
            return;
        }

        StartCoroutine(CoMakeRealGame());
    }

    //IEnumerator COMakeRealGame()
    //{
    //    for(_gameIndex = 0; _gameIndex < _realWorldInfo.dialog.Count; _gameIndex++) { 
    //        MakeRealGame();
    //        yield return new WaitForSeconds(_makeGameDelay);
    //    }
    //}
}
