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
    private bool _canPressKey = true;  

    private float _remainingTime;
    private float _currentGauge;

    private List<KeyButton> _requiredKeys = new List<KeyButton>();
    int _keyCount = 0;

    public float CheetCode = 1f;

    private KeySpriteFactory _keySpriteFactory;
    public void Init(MiniGameInfo miniGameInfo, SpawnInfo spawnInfo, KeySpriteFactory keySpriteFactory)
    {
        //미니게임 정보 설정
        _miniGameInfo = miniGameInfo;
        _bubbleText.text = _miniGameInfo.dialog;
        _currentGauge = _miniGameInfo.startGauge;
        _remainingTime = _miniGameInfo.limitTime + 5f;
        _keySpriteFactory = keySpriteFactory;

        SetKeyPressButton();

        _spawnInfo = spawnInfo;

        if (_spawnInfo.isLeft)
        {
            _bubbleImage.transform.Rotate(0, 180, 0);
        }

        // 초기화
        UpdateUI();
    }

    private void SetKeyPressButton()
    {
        if (_miniGameInfo.requiredKeys == null) return;

        _requiredKeys.Clear(); // 초기화
        foreach (KeyCode keyCode in _miniGameInfo.requiredKeys)
        {
            // KeyButton 생성
            KeyButton keyButton = Managers.UI.MakeSubItem<KeyButton>(_minigameGaugeBar.transform, "KeyButton");
            keyButton.OnKeyPressed += OnKeyPressed; // 입력 이벤트 연결
            keyButton.Init(keyCode, _keySpriteFactory.GetKeySprite(keyCode)); // KeyCode 설정
            _requiredKeys.Add(keyButton); // 리스트에 추가
        }

        if (_requiredKeys.Count > 0)
        {
            _canPressKey = false; // Space 비활성화
            _keyCount = _requiredKeys.Count;
            SetKeyButtonPosition();
        }
    }

    private void SetKeyButtonPosition()
    {
        int keyCount = _requiredKeys.Count;

        float length = _minigameGaugeBar.rectTransform.rect.width;
        float gap = length / keyCount;
        float startX = -length / 2 + gap / 2;

        for (int i = 0; i < keyCount; i++)
        {
            _requiredKeys[i].transform.localPosition = new Vector3(startX + gap * i, 0, 0);
        }
    }

    private void OnKeyPressed()
    {
        _keyCount--;
        if(_keyCount == 0)
        {
            _canPressKey = true;
        }
    }

    // 텍스트 효과 
    private IEnumerator ShowText()
    {
        yield return null;
        // 모자이크 효과
        //while (_bubbleText.outlineWidth > 0f)
        //{
        //    _bubbleText.outlineWidth -= _mosaicRemoveSpeed * Time.deltaTime;
        //    yield return null;
        //}
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
            if (_canPressKey && Input.GetKeyDown(KeyCode.Space))
            {
                ChangeGauge(_miniGameInfo.perIncreaseGauge);
            }
        }
        else if (_miniGameInfo.difficulty == MiniGameDifficulty.Hard)
        {
            // 동시에 특정 키를 누르는 난이도
            if (_canPressKey && Input.GetKeyDown(KeyCode.Space))
            {
                ChangeGauge(_miniGameInfo.perIncreaseGauge);
            }
        }
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