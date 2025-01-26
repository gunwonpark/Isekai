using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniGame : UI_Popup
{
    [SerializeField] private Image _bubbleImage;
    [SerializeField] private TextMeshProUGUI _bubbleText;
    [SerializeField] private Image _minigameGaugeBar;

    private SpawnInfo _spawnInfo;

    [Tooltip("미니게임 정보")]
    [SerializeField] private MiniGameInfo _miniGameInfo;

    [Tooltip("타이머 텍스트")]
    [SerializeField] private TextMeshProUGUI _remainTimeText;

    [SerializeField] private float _mosaicRemoveSpeed = 1.0f;

    private bool _isGameEnd = false;
    private bool _isGameStart = false;

    private float _remainingTime;
    private float _currentGauge;

    private List<KeyCode> _requiredKeys = new List<KeyCode>(); // 난이도별로 필요한 키

    public float CheetCode = 1f;
    public void Init(MiniGameInfo miniGameInfo, SpawnInfo spawnInfo)
    {        
        //미니게임 정보 설정
        _miniGameInfo = miniGameInfo;
        _bubbleText.text = _miniGameInfo.dialog;
        _currentGauge = _miniGameInfo.startGauge;
        _remainingTime = _miniGameInfo.limitTime + 5f;

        _spawnInfo = spawnInfo;

        if (_spawnInfo.isLeft)
        {
            _bubbleImage.transform.Rotate(0, 180, 0);
        }

        // 초기화
        


        GenerateKeysByDifficulty();
        UpdateUI();
    }

    private IEnumerator ShowText()
    {
        while (_bubbleText.outlineWidth > 0f)
        {
            _bubbleText.outlineWidth -= _mosaicRemoveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (_isGameEnd) return;

        // 시간 감소
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0)
        {
            EndMiniGame(false);
            return;
        }

        // 게이지 감소
        ChangeGauge(_miniGameInfo.perDecreaseGauge * Time.deltaTime);
        if (_currentGauge <= 0)
        {
            EndMiniGame(false);
            return;
        }

        // 키 입력 처리
        HandleKeyPress();

        // UI 업데이트
        UpdateUI();
    }

    private void HandleKeyPress()
    {
        if(!_isGameStart) return;


        if (_miniGameInfo.difficulty == MiniGameDifficulty.Easy)
        {
            // 스페이스바를 누르면 게이지 증가
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeGauge(_miniGameInfo.perIncreaseGauge + CheetCode);
            }
        }
        else if (_miniGameInfo.difficulty == MiniGameDifficulty.Normal)
        {
            // 모든 키를 누른 후 스페이스바를 누르는 난이도
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeGauge(_miniGameInfo.perIncreaseGauge);
                ResetKeyStates();
            }
        }
        else if (_miniGameInfo.difficulty == MiniGameDifficulty.Hard)
        {
            // 동시에 특정 키를 누르는 난이도
            if (AreAllKeysPressed())
            {
                ChangeGauge(_miniGameInfo.perIncreaseGauge);
                ResetKeyStates();
            }
        }
    }

    private void GenerateKeysByDifficulty()
    {
        _requiredKeys.Clear();
        if (_miniGameInfo.difficulty == MiniGameDifficulty.Normal || _miniGameInfo.difficulty == MiniGameDifficulty.Hard)
        {
            int keyCount = UnityEngine.Random.Range(3, 7); // 3~6개의 키를 랜덤으로 선택
            Array values = Enum.GetValues(typeof(KeyCode));
            for (int i = 0; i < keyCount; i++)
            {
                KeyCode randomKey = (KeyCode)values.GetValue(UnityEngine.Random.Range(0, values.Length));
                _requiredKeys.Add(randomKey);
            }
        }
    }

    private bool AreAllKeysPressed()
    {
        foreach (var key in _requiredKeys) 
        {
            if (!Input.GetKey(key))
                return false;
        }
        return true;
    }

    private void ResetKeyStates()
    {
        // 키 상태 초기화 (필요 시)
    }

    private void ChangeGauge(float amount)
    {
        _currentGauge += amount;
        if (_currentGauge >= 100f)
        {
            EndMiniGame(true);
        }
    }

    private void UpdateUI()
    {
        _minigameGaugeBar.fillAmount = _currentGauge / 100f;
        _remainTimeText.text = $"남은 시간: {_remainingTime:F1}초";
    }

    private void EndMiniGame(bool isSuccess)
    {
        if (_isGameEnd) return;

        _isGameEnd = true;

        if (isSuccess)
        {
            Debug.Log("미니게임 성공! 행복도가 상승합니다.");
            Managers.Happy.ChangeHappiness(_miniGameInfo.succedGauge + CheetCode);
        }
        else
        {
            Debug.Log("미니게임 실패! 행복도가 감소합니다.");
            Managers.Happy.ChangeHappiness(_miniGameInfo.failGauge - CheetCode);
        }

        // 게임 종료 로직
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isGameStart) return;

        _isGameStart = true;

        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ShowText());
        }
    }

    public override void Init()
    {
        // 추가 초기화가 필요한 경우 여기에 작성
    }
}