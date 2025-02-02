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
    public void Init(RealWorldInfo realWorldInfo)
    {
        _realWorldInfo = realWorldInfo;
        _gameCount = _realWorldInfo.dialog.Count;

        StartCoroutine(COMakeRealGame());
    }

    public void MakeRealGame()
    {
        UI_Bubble ui_Bubble = Managers.UI.MakeSubItem<UI_Bubble>(this.transform);
        Vector2 pos = new Vector2(Random.Range(-6.0f, 6.0f), 10f);
        ui_Bubble.transform.position = pos;

        ui_Bubble.Init(_realWorldInfo.dialog[_gameIndex], _realWorldInfo.score[_gameIndex]);

        ui_Bubble.OnCollisionEvent += Ui_Bubble_OnCollisionEvent;

    }

    private void Ui_Bubble_OnCollisionEvent()
    {
        _gameCount--;

        if (Managers.Happy.Happiness <= 0)
        {
            EndGame(false);
        }
        else if(_gameCount == 0)
        {
            EndGame(true);
        }
    }

    private void EndGame(bool isSucess)
    {
        StopAllCoroutines();

        Debug.Log("EndGame");
        //주인공은 오른쪽으로 이동하고 카메라는 고정
        //화면은 천천히 faded out되고 fade in 되면서 도서관 씬으로 이동
        //상관없이 도서관 씬으로 이동
    }

    IEnumerator COMakeRealGame()
    {
        for(_gameIndex = 0; _gameIndex < _realWorldInfo.dialog.Count; _gameIndex++) { 
            MakeRealGame();
            yield return new WaitForSeconds(_makeGameDelay);
        }
    }
}
