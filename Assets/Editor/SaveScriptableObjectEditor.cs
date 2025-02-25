using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SaveScriptableObjectToJsonEditor : Editor
{
    static string savePath = Application.dataPath + "/Datas";

    [MenuItem("ScriptableObject/Save")]
    public static void Save()
    {
        Object obj = Selection.activeObject;

        if (obj == null) return;

        if (obj is not ScriptableObject) return;

        obj = obj as ScriptableObject;

        string json = JsonUtility.ToJson(obj, true);
        string path = System.IO.Path.Combine(savePath, obj.name + ".json");

        if(!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }

        System.IO.File.WriteAllText(path, json);

        Debug.Log($"Save Complete : {json}");
        AssetDatabase.Refresh();
    }

    [MenuItem("ScriptableObject/Load")]
    public static void Load()
    {
        Object obj = Selection.activeObject;

        if (obj == null) return;
        if (obj is not ScriptableObject) return;

        obj = obj as ScriptableObject;
        string path = System.IO.Path.Combine(savePath, obj.name + ".json");
        string json = System.IO.File.ReadAllText(path);

        JsonUtility.FromJsonOverwrite(json, obj);
        Debug.Log("Load Complete");
        AssetDatabase.Refresh();
    }
}
