using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test_G))]
public class Test_GEditor : Editor
{
    Test_G testG;

    private void OnEnable()
    {
        testG = (Test_G)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Shake Camera"))
        {
            //testG.ShakeCamera();
        }
    }
}
