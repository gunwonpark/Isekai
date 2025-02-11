using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Data
public struct MiniGameInfo
{
    public MiniGameDifficulty difficulty;
    public int startGauge;
    public int perDecreaseGauge;
    public int perIncreaseGauge;
    public int succedGauge;
    public int failGauge;
    public int runGauge;
    public int limitTime;

    public List<KeyCode> requiredKeys; // 필요한 키 리스트
    public int requiredKeyCount; // 필요한 키 개수
    public bool canPressConcurrent; // 동시에 누를 수 있는지 여부
    public string dialog;
}

#endregion

public struct SpawnInfo
{
    public Vector2 position;
    public bool isLeft;
}

public class MiniGameFactory : MonoBehaviour
{
    [SerializeField] private UI_MiniGame _miniGame;
    [SerializeField] private WorldInfo _worldInfo;
    [SerializeField] private GridSystem _gridSystem;

    [SerializeField] private GameObject _leftExclamation;
    [SerializeField] private GameObject _rightExclamation;

    [SerializeField] private KeySpriteFactory _keySpriteFactory;

    [SerializeField] private Transform target;
    [SerializeField] private Transform _leftPosition;
    [SerializeField] private Transform _rightPosition;

    [SerializeField] private float _minY = -2f;
    [SerializeField] private float _maxY = 2f;
    [SerializeField] private float _spawnDelay = 4f;

    private bool _isGameEnd = false;

    private Queue<UI_MiniGame> _miniGameQueue = new Queue<UI_MiniGame>();

    public event Action<bool> OnGameEnd;

    public void Init(WorldType worldType)
    {
        SetWorld(worldType);
    
        Managers.Happy.OnHappinessChanged += CreateControll;

        _keySpriteFactory = new KeySpriteFactory();
        _keySpriteFactory.Init();

        _gridSystem.Init(target);

        StartCoroutine(CreateMiniGame());
    }

    private void Update()
    {
        if(_gridSystem.IsBubbleEmpty)
        {
            Debug.Log("IsBubbleEmpty");
            bool isLeft = false;
            bool isRight = false;
            foreach(UI_MiniGame _miniGame in _miniGameQueue)
            {
                if(_miniGame.gameObject.activeSelf)
                {
                    isLeft = _miniGame.transform.position.x < target.position.x;
                    isRight = _miniGame.transform.position.x > target.position.x;
                }
            }

            if (isLeft)
            {
                _leftExclamation.SetActive(true);
            }
            else
            {
                _leftExclamation.SetActive(false);
            }
            if (isRight)
            {
                _rightExclamation.SetActive(true);
            }
            else
            {
                _rightExclamation.SetActive(false);
            }
        }
        else
        {
            _leftExclamation.SetActive(false);
            _rightExclamation.SetActive(false);
        }
    }


    public IEnumerator CreateMiniGame()
    {
        while (true)
        {
            SpawnInfo spawnInfo = GetRandomPosition();
            UI_MiniGame miniGame = Instantiate(_miniGame, spawnInfo.position, Quaternion.identity);
            MiniGameInfo miniGameInfo = _worldInfo.GetRandomMiniGameInfo();
            _miniGameQueue.Enqueue(miniGame);
            miniGame.Init(miniGameInfo, spawnInfo, _keySpriteFactory);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    // target을 기준으로 일정 거리만큼 떨어진 곳에 생성
    private SpawnInfo GetRandomPosition()
    {
        float randomY = UnityEngine.Random.Range(_minY, _maxY);

        if(_gridSystem.TryGetEmptyPosition(out Vector2 spawnPos))
        {
        }

        bool isLeft = spawnPos.x < target.position.x;

        return new SpawnInfo() { position = spawnPos + new Vector2(0, randomY), isLeft = isLeft };
    }

    // 행복도에 따른 게임종료여부 판단
    private void CreateControll(float happiness)
    {
        if(_isGameEnd) return;


        if (happiness <= 0 || happiness >= 100)
        {
            _isGameEnd = true;
            StopAllCoroutines();
            foreach (var miniGame in _miniGameQueue)
            {
                miniGame.gameObject.SetActive(false);
            }

            OnGameEnd?.Invoke(happiness >= 100);
        }
    }

    // 월드 정보 설정
    private void SetWorld(WorldType worldType)
    {
        switch (worldType)
        {
            case WorldType.Vinter:
                _worldInfo = new VinterWorldInfo();
                break;
            case WorldType.Chaumm:
                _worldInfo = new ChaummWorldInfo();
                break;
            case WorldType.Gang:
                _worldInfo = new GangWorldInfo();
                break;
            case WorldType.Pelmanus:
                _worldInfo = new PelmanusWorldInfo();
                break;
        }
    }

    private void OnDestroy()
    {
        Managers.Happy.OnHappinessChanged -= CreateControll;
    }
}
