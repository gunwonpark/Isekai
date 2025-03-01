using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatUI : MonoBehaviour
{
    public Button playerSet;
    public Slider playerSpeed;
    public TextMeshProUGUI playerSpeedText;
    public float playerSpeedValue
    {
        get => playerSpeed.value * 10f;
        set
        {
            playerSpeedText.text = (value * 10).ToString();
        }
    }

    public Button StageSet;
    public Slider perDecreaseGauge;
    public Slider perIncreaseGauge;
    public Slider LimitTime;

    public TextMeshProUGUI perDecreaseGaugeText;
    public TextMeshProUGUI perIncreaseGaugeText;
    public TextMeshProUGUI LimitTimeText;


    private int _perDecreaseGaugeValue;

    public int perDecreaseGaugeValue
    {
        get => -_perDecreaseGaugeValue;
        set
        {
            _perDecreaseGaugeValue = value;
            perDecreaseGaugeText.text = _perDecreaseGaugeValue.ToString();
        }
    }

    private int _perIncreaseGaugeValue;

    public int perIncreaseGaugeValue
    {
        get => _perIncreaseGaugeValue;
        set
        {
            _perIncreaseGaugeValue = value;
            perIncreaseGaugeText.text = _perIncreaseGaugeValue.ToString();
        }
    }

    private int _limitTimeValue;
    public int LimitTimeValue
    {
        get => _limitTimeValue;
        set
        {
            _limitTimeValue = value;
            LimitTimeText.text = _limitTimeValue.ToString();
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerSet.onClick.AddListener(SetPlayerSpeed);
        playerSpeed.onValueChanged.AddListener((value) => playerSpeedValue = value);

        StageSet.onClick.AddListener(SetStageData);
        perDecreaseGauge.onValueChanged.AddListener((value) => perDecreaseGaugeValue = Mathf.RoundToInt(value * 30));
        perIncreaseGauge.onValueChanged.AddListener((value) => perIncreaseGaugeValue = Mathf.RoundToInt(value * 30));
        LimitTime.onValueChanged.AddListener((value) => LimitTimeValue = Mathf.RoundToInt(value * 20));
    }

    private void SetStageData()
    {
        WorldInfo worldInfo = Managers.DB.GetGameSceneData(Managers.World.CurrentWorldType);
        GameSceneData gameSceneData = new GameSceneData()
        {
            worldType = worldInfo.worldType,
            difficulty = worldInfo.difficulty,
            startGauge = worldInfo.startGauge,
            perDecreaseGauge = new List<int> { perDecreaseGaugeValue, perDecreaseGaugeValue, perDecreaseGaugeValue },
            perIncreaseGauge = new List<int> { perIncreaseGaugeValue, perIncreaseGaugeValue, perIncreaseGaugeValue },
            succedGauge = worldInfo.succedGauge,
            failGauge = worldInfo.failGauge,
            runGauge = worldInfo.runGauge,
            limitTime = new List<int> { LimitTimeValue, LimitTimeValue, LimitTimeValue },
            dialog = worldInfo.dialog,
            requireKeys = worldInfo.requireKeys,
            requiredKeyCount = worldInfo.requiredKeyCount,
            canPressConcurrent = worldInfo.canPressConcurrent
        };

        Managers.DB.SetGameSceneData(Managers.World.CurrentWorldType, gameSceneData);
    }

    private void SetPlayerSpeed()
    {
        PlayerData playerData = Managers.DB.GetPlayerData();
        playerData.moveSpeed[0] = playerSpeedValue;
        playerData.moveSpeed[1] = playerSpeedValue;
        playerData.moveSpeed[2] = playerSpeedValue;
        Managers.DB.SetPlayerData(playerData);
    }
}
