using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UI_MiniGame : UI_Popup
{
    
    [SerializeField] private Image _bubbleImage;
    [SerializeField] private TextMeshProUGUI _bubbleText;
    [SerializeField] private Image _minigameGaugeBar;
    [SerializeField] private Image _positionImage;

    private string _originalText;

    private SpawnInfo _spawnInfo;

    [Tooltip("미니게임 정보")]
    [SerializeField] private MiniGameInfo _miniGameInfo;

    [Tooltip("타이머 텍스트")]
    [SerializeField] private TextMeshProUGUI _remainTimeText;

    [SerializeField] private float _mosaicRemoveSpeed = 1.0f;

    [SerializeField] private string[] _maskCharacters = { "#", "*", "@", "$", "%", "&", "!" }; // 특수문자 집합

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
        _originalText = _miniGameInfo.dialog;
        _bubbleText.text = GetRandomMaskedText(_miniGameInfo.dialog.Length);
        _currentGauge = _miniGameInfo.startGauge;
        _remainingTime = _miniGameInfo.limitTime + 5f;
        _keySpriteFactory = keySpriteFactory;

        SetKeyPressButton();

        _spawnInfo = spawnInfo;

        if (!_spawnInfo.isLeft)
        {
            _bubbleImage.transform.Rotate(0, 180, 0);
        }

        // 초기화
        UpdateUI();
    }

    private string GetRandomMaskedText(int length)
    {
        StringBuilder randomText = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            randomText.Append(_maskCharacters[UnityEngine.Random.Range(0, _maskCharacters.Length)]);
        }
        return randomText.ToString();
    }

    // 텍스트 효과 
    private IEnumerator ShowText()
    {

        int textLength = _originalText.Length;
        float totalDuration = 1.0f; // 전체 변환 시간
        float delayPerChar = totalDuration / textLength; // 각 글자당 딜레이

        // StringBuilder를 사용하여 효율적으로 텍스트 구성
        StringBuilder currentText = new StringBuilder(_bubbleText.text);

        List<int> order = new List<int>();
        for (int i = 0; i < textLength; i++)
        {
            order.Add(i);
        }

        order.Shuffle();

        for (int i = 0; i < textLength; i++)
        {
            int index = order[i];
            // 원래 텍스트의 i번째 글자를 업데이트
            currentText[index] = _originalText[index];

            _bubbleText.text = currentText.ToString();
            yield return new WaitForSeconds(delayPerChar);
        }
    }

    private void SetKeyPressButton()
    {
        if (_miniGameInfo.requiredKeys == null) return;

        _requiredKeys.Clear(); // 초기화

        int requiredKeyCount = _miniGameInfo.requiredKeyCount;

        int adder = 0;
        //동시 입력이 가능할 경우
        if (_miniGameInfo.canPressConcurrent)
        {
            List<KeyCode> keyCode = new List<KeyCode>();

            int randomKeyCount = UnityEngine.Random.Range(2, 4);
            for (int i = _miniGameInfo.requiredKeys.Count - 1; i >= _miniGameInfo.requiredKeys.Count - randomKeyCount; i--)
            {
                keyCode.Add(_miniGameInfo.requiredKeys[i]);
            }

            if(randomKeyCount == 2)
            {
                // KeyButton 생성
                TwoKeyButton keyButton = Managers.UI.MakeSubItem<TwoKeyButton>(_minigameGaugeBar.transform, "TwoKeyButton");
                keyButton.OnKeyPressed += OnKeyPressed; // 입력 이벤트 연결
                keyButton.Init(keyCode[0], keyCode[1], _keySpriteFactory.GetKeySprite(keyCode[0]), _keySpriteFactory.GetKeySprite(keyCode[1])); // KeyCode 설정
                _requiredKeys.Add(keyButton); // 리스트에 추가
            }
            else if(randomKeyCount == 3)
            {
                // KeyButton 생성
                ThreeKeyButton keyButton = Managers.UI.MakeSubItem<ThreeKeyButton>(_minigameGaugeBar.transform, "ThreeKeyButton");
                keyButton.OnKeyPressed += OnKeyPressed; // 입력 이벤트 연결
                keyButton.Init(keyCode[0], keyCode[1], keyCode[2], _keySpriteFactory.GetKeySprite(keyCode[0]), _keySpriteFactory.GetKeySprite(keyCode[1]), _keySpriteFactory.GetKeySprite(keyCode[2])); // KeyCode 설정
                _requiredKeys.Add(keyButton); // 리스트에 추가
            }

            adder = 1;
        }

        for(int i = 0; i < requiredKeyCount - adder; i++)
        {
            // KeyButton 생성
            KeyButton keyButton = Managers.UI.MakeSubItem<KeyButton>(_minigameGaugeBar.transform, "KeyButton");
            keyButton.OnKeyPressed += OnKeyPressed; // 입력 이벤트 연결
            KeyCode keyCode = _miniGameInfo.requiredKeys[i];
            keyButton.Init(keyCode, _keySpriteFactory.GetKeySprite(keyCode)); // KeyCode 설정
            _requiredKeys.Add(keyButton); // 리스트에 추가
        }

        if (requiredKeyCount > 0)
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

        if (_currentGauge < 0)
        {
            _currentGauge = 0;
            EndMiniGame(false);
        }

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
            foreach (var key in _requiredKeys)
            {
                key.EnableKeyPress();
            }
            StartCoroutine(ShowText());
        }
    }

    public void ShowExclamation(Vector3 position)
    {
        if (!_positionImage.gameObject.activeSelf) _positionImage.gameObject.SetActive(true);

        _positionImage.transform.position = position;
    }

    public void HideExclamation()
    {
        if (!_positionImage.gameObject.activeSelf) return;

        _positionImage.gameObject.SetActive(false);
    }
    public override void Init()
    {
        // 추가 초기화가 필요한 경우 여기에 작성
    }
}