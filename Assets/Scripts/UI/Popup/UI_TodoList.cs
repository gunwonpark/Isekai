using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TodoList : MonoBehaviour
{
    private LoadingGameSceneData _data;

    [SerializeField] private Transform _toggleGroup;
    [SerializeField] private TMP_Text _diaryText;

    private List<Sub_Toggle> _toggles = new List<Sub_Toggle>();
    int index = 0;
    public void Init(LoadingGameSceneData data)
    {
        _data = data;
        for(int i = 0; i < _data.todoList.Count; i++)
        {
            Sub_Toggle go = Managers.UI.MakeSubItem<Sub_Toggle>(_toggleGroup);
            go.transform.parent = _toggleGroup;
            go.Init(_data.todoList[i], i);
            _toggles.Add(go);
            _toggles[i].Toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    StartCoroutine(OnToggleOn());
                }
            });
            go.gameObject.SetActive(false);
        }
        _toggles[0].gameObject.SetActive(true);
    }

    private IEnumerator OnToggleOn()
    {
        Debug.Log(index);
        yield return StartCoroutine(_toggles[index].Text.CoTypingEffect(_data.todoList[index], 0.1f));

        if(index == _toggles.Count - 1)
        {
            _diaryText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            StartCoroutine(_diaryText.CoTypingEffect(_data.diary, 0.05f));
        }
        else
        {
            _toggles[index + 1].gameObject.SetActive(true);
        }
        index++;
    }
}
