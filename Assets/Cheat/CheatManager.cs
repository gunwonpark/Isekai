using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CheatManager : MonoBehaviour
{
    public TMP_Dropdown _sceneDropdown;
    public TMP_Dropdown _sceneType;
    public Button SetWorldButton;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  
    }

    private void Start()
    {
        SetWorldButton.onClick.AddListener(SetWorld);
        _sceneDropdown.ClearOptions();
        _sceneType.ClearOptions();
        _sceneDropdown.AddOptions(Enum.GetNames(typeof(WorldType)).ToList());
        _sceneType.AddOptions(Enum.GetNames(typeof(Scene)).ToList());
    }

    private void SetWorld()
    {
        Managers.World.CurrentWorldType = (WorldType)_sceneDropdown.value;
        Managers.Scene.LoadScene((Scene)_sceneType.value);
    }
}
