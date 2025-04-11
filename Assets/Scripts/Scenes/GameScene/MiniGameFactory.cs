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
public struct SpawnInfo
{
    public Vector2 position;
    public bool isLeft;
}
#endregion


/// <summary>
/// 1. 미니게임 생성
/// 2. 정보 초기화
/// 
/// </summary>

public class MiniGameFactory : MonoBehaviour
{
    [SerializeField] private UI_MiniGame _miniGame;

    [SerializeField] private GridSystem _gridSystem;
    [SerializeField] private KeySpriteFactory _keySpriteFactory;

    [Header("플레이어 정보")]
    [SerializeField] private Transform _target;

    // 리펙토링 고려대상
    [SerializeField] private GameObject _leftExclamation;
    [SerializeField] private GameObject _rightExclamation;

    [Header("미니게임 생성 정보")]
    [SerializeField] private float _minBubbleYPos = 0f;
    [SerializeField] private float _maxBubbleYPos = 2f;
    [SerializeField] private float _spawnDelay = 4f;

    [SerializeField] private float _waitBeforeGameStartTime = 1f;       // 미니게임 생성전 대기시간

    private Queue<UI_MiniGame> _miniGameQueue = new Queue<UI_MiniGame>();

    private WorldInfo _worldInfo;

    public event Action<bool> OnGameEnd;

    private bool _isGameEnd = false;
    private int successCount = 0;


    public void Init()
    {
        // 월드 데이터 가져오기
        _worldInfo = Managers.World.GetWorldInfo();

        Managers.Happy.OnHappinessChanged += CheckMiniGameEnd;

        // 키 스프라이트 매칭 작업
        _keySpriteFactory = new KeySpriteFactory();
        _keySpriteFactory.Init();

        // 말풍선 위치 탐지 작업
        _gridSystem.Init(_target);

        StartCoroutine(CreateMiniGame());
    }
    public bool IsBubbleEmpty
    {
        get
        {
            float height = Camera.main.orthographicSize * 2f;
            float width = height * Camera.main.aspect;

            Collider2D col = Physics2D.OverlapBox(Camera.main.transform.position, new Vector2(width, height), 0, LayerMask.GetMask("UI"));
            return col == null;
        }
    }

    private void Update()
    {
        bool isLeft = false;
        bool isRight = false;

        if (IsBubbleEmpty)
        {
            // 활성화된 게임 오브젝트에 대해서만 처리
            foreach (UI_MiniGame _miniGame in _miniGameQueue)
            {
                if (_miniGame.gameObject.activeSelf)
                {
                    isLeft |= _miniGame.transform.position.x < _target.position.x;
                    isRight |= _miniGame.transform.position.x > _target.position.x;
                }
            }

            _leftExclamation.SetActive(isLeft);
            _rightExclamation.SetActive(isRight);
        }
        else
        {
            _leftExclamation.SetActive(false);
            _rightExclamation.SetActive(false);
        }
    }


    public IEnumerator CreateMiniGame()
    {
        yield return WaitForSecondsCache.Get(_waitBeforeGameStartTime);

        while (true)
        {
            
            if(TryGetRandomPosition(out Vector2 randomPos)){
                bool isLeftSide = randomPos.x < _target.position.x;

                SpawnInfo spawnInfo = new SpawnInfo
                {
                    position = randomPos,
                    isLeft = isLeftSide
                };

                MiniGameInfo miniGameInfo = _worldInfo.GetRandomMiniGameInfo();

                UI_MiniGame miniGame = Instantiate(_miniGame, spawnInfo.position, Quaternion.identity);
                _miniGameQueue.Enqueue(miniGame);

                miniGame.Init(miniGameInfo, spawnInfo, _keySpriteFactory);

                // 이 부분이 심히 거슬린다
                // 세계가 바뀌면?
                // 펠마누스 세계에서만 포스트 프로세싱과 7개의 미니게임을 성공했을 시 게임 종료
                if(Managers.World.CurrentWorldType == WorldType.Pelmanus)
                {
                    miniGame.onMiniGameSucced += () =>
                    {
                        GameSceneEx scene = Managers.Scene.CurrentScene as GameSceneEx;
                        scene.SetPostProcessing(successCount);

                        if(successCount == 7)
                        {
                            GameEnd(true);
                        }
                    };
                }
            }

            yield return WaitForSecondsCache.Get(_spawnDelay);
        }
    }

    private bool TryGetRandomPosition(out Vector2 randomPos)
    {
        float randomY = UnityEngine.Random.Range(_minBubbleYPos, _maxBubbleYPos);

        if(_gridSystem.TryGetEmptyPosition(out Vector2 spawnPos) == false)
        {
            Debug.Log("모든 공간이 찼습니다.");
            randomPos = Vector2.zero;
            return false;
        }

        randomPos = spawnPos + new Vector2(0, randomY);
        return true;
    }

    // 행복도에 따른 게임종료여부 판단
    private void CheckMiniGameEnd(float happiness)
    {
        if(_isGameEnd) return;

        if (happiness <= 0 || happiness >= 100)
        {
            GameEnd(happiness >= 100);
        }
    }

    private void GameEnd(bool isSuccess)
    {
        _isGameEnd = true;
        StopAllCoroutines();

        foreach (var miniGame in _miniGameQueue)
        {
            miniGame.gameObject.SetActive(false);
        }

        OnGameEnd?.Invoke(isSuccess);
    }

    private void OnDestroy()
    {
        Managers.Happy.OnHappinessChanged -= CheckMiniGameEnd;
    }
}
