using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSceneEx))]
public class GameSceneExEditor : Editor
{
    private GameSceneEx _gameSceneEx;
    private void OnEnable()
    {
        _gameSceneEx = target as GameSceneEx; 
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GameEnd"))
        {
            _gameSceneEx.GameOver(true);
        }

        if (GUILayout.Button("GameOver"))
        {
            _gameSceneEx.GameOver(false);
        }
    }

}
