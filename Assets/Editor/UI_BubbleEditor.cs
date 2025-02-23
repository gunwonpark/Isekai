using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UI_Bubble))]
public class UI_BubbleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        UI_Bubble bubble = target as UI_Bubble;
        if (GUILayout.Button("Init"))
        {
            bubble.Init(bubble.testText, 10);
        }
    }
}
