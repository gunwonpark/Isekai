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
            playerSpeedText.text = (value * 10).ToString("F2");
        }
    }

    public Button StageSet;
    public Slider perDecreaseGauge;
    public Slider perIncreaseGauge;
    public Slider LimitTime;
    public Slider Success;
    public Slider Fail;

    public TextMeshProUGUI perDecreaseGaugeText;
    public TextMeshProUGUI perIncreaseGaugeText;
    public TextMeshProUGUI LimitTimeText;
    public TextMeshProUGUI SuccessText;
    public TextMeshProUGUI FailText;

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

    private int _successValue;

    public int SuccessValue
    {
        get => _successValue;
        set
        {
            _successValue = value;
            SuccessText.text = _successValue.ToString();
        }
    }

    private int _failValue;

    public int FailValue
    {
        get => -_failValue;
        set
        {
            _failValue = value;
            FailText.text = _failValue.ToString();
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        playerSet.onClick.AddListener(SetPlayerSpeed);
        playerSpeed.onValueChanged.AddListener((value) => playerSpeedValue = value);

        StageSet.onClick.AddListener(SetStageData);
        perDecreaseGauge.onValueChanged.AddListener((value) => perDecreaseGaugeValue = Mathf.RoundToInt(value * 30));
        perIncreaseGauge.onValueChanged.AddListener((value) => perIncreaseGaugeValue = Mathf.RoundToInt(value * 30));
        LimitTime.onValueChanged.AddListener((value) => LimitTimeValue = Mathf.RoundToInt(value * 20));
        Success.onValueChanged.AddListener((value) => SuccessValue = Mathf.RoundToInt(value * 50));
        Fail.onValueChanged.AddListener((value) => FailValue = Mathf.RoundToInt(value * 50));
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
            succedGauge = new List<int> { SuccessValue, SuccessValue, SuccessValue },
            failGauge = new List<int> { FailValue, FailValue, FailValue },
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
