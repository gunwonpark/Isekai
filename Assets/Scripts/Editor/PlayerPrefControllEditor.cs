using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefControllEditor : Editor
{
    [MenuItem("PlayerPref/DeleteAll")]
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log(PlayerPrefs.GetInt("WorldType", -1));
    }
}
