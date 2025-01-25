using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<KeyCode> requiredKeys;
    public List<int> requiredKeyCount;
    public List<bool> canPressConcurrent;
    public string dialog;
}

public abstract class WorldInfo
{
    public WorldType worldType;

    public List<int> difficulty = new List<int>();
    public List<int> startGauge = new List<int>();
    public List<int> perDecreaseGauge = new List<int>();
    public List<int> perIncreaseGauge = new List<int>();
    public List<int> succedGauge = new List<int>();
    public List<int> failGauge = new List<int>();
    public List<int> runGauge = new List<int>();
    public List<int> limitTime = new List<int>();

    public readonly List<string> dialog = new List<string>();

    public WorldInfo(WorldType worldType)
    {
        this.worldType = worldType;
    }

    public virtual MiniGameInfo GetRandomMiniGameInfo()
    {
        // 랜덤 텍스트 결정
        int index = UnityEngine.Random.Range(0, dialog.Count);

        // 랜덤 난이도 문제 결정
        int difficultyIndex = UnityEngine.Random.Range(0, dialog.Count);

        if (difficultyIndex < difficulty[0])
        {
            difficultyIndex = 0;
        }
        else if (difficultyIndex < difficulty[0] + difficulty[1])
        {
            difficultyIndex = 1;
        }
        else
        {
            difficultyIndex = 2;
        }

        MiniGameInfo miniGameInfo = new MiniGameInfo()
        {
            difficulty = (MiniGameDifficulty)difficultyIndex,
            startGauge = startGauge[difficultyIndex],
            perDecreaseGauge = perDecreaseGauge[difficultyIndex],
            perIncreaseGauge = perIncreaseGauge[difficultyIndex],
            succedGauge = succedGauge[difficultyIndex],
            failGauge = failGauge[difficultyIndex],
            runGauge = runGauge[difficultyIndex],
            limitTime = limitTime[difficultyIndex],
            dialog = dialog[index]
        };

        return miniGameInfo;
    }
}

public class VinterWorldInfo : WorldInfo
{
    public VinterWorldInfo() : base(WorldType.Vinter)
    {
        difficulty.AddRange(new List<int>() { 6, 0, 0 });
        startGauge.AddRange(new List<int>() { 50, 50, 50 });
        perDecreaseGauge.AddRange(new List<int>() { -10, -10, -10 });
        perIncreaseGauge.AddRange(new List<int>() { 3, 3, 3 });
        succedGauge.AddRange(new List<int>() { 40, 40, 40 });
        failGauge.AddRange(new List<int>() { -10, -10, -10 });
        runGauge.AddRange(new List<int>() { -20, -20, -20 });
        limitTime.AddRange(new List<int>() { 4, 4, 4 });

        dialog.AddRange(new List<string>
        {
            "역시 명문 가문의 품격을 갖춘 완벽한 인물이셔!",
            "공작님의 재능은 신이 내리신 선물이야.",
            "공작님의 외모는 신이 내리신 예술 작품 같아.",
            "공작님을 칭송하는 것만으로도 영광이야!",
            "그의 존재 자체가 이 국가에 큰 축복이지.",
            "모든 영애가 공작님의 외모만 보면 눈물을 흘린다지?"
        });
    }
}

public class ChaummWorldInfo : WorldInfo
{
    public readonly List<KeyCode> requireKeys = new List<KeyCode>()
    { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D };
    public List<int> requiredKeyCount = new List<int>() { 0, 3, 0 };
    public List<bool> canPressConcurrent = new List<bool>() { false, false, false };
    public ChaummWorldInfo() : base(WorldType.Chaumm)
    {
        difficulty.AddRange(new List<int>() { 3, 4, 0 });
        startGauge.AddRange(new List<int>() { 50, 40, 50 });
        perDecreaseGauge.AddRange(new List<int>() { -10, -15, -10 });
        perIncreaseGauge.AddRange(new List<int>() { 5, 5, 3 });
        succedGauge.AddRange(new List<int>() { 30, 30, 40 });
        failGauge.AddRange(new List<int>() { -10, -10, -10 });
        runGauge.AddRange(new List<int>() { -20, -20, -20 });
        limitTime.AddRange(new List<int>() { 4, 4, 4 });

        dialog.AddRange(new List<string>
        {
            "왜 아직 너를 위한 나라가 없는거야!!",
            "김 묻었어. 잘생김.",
            "내면의 아름다움이란 얼마나 하찮은 것인가...",
            "너는 린스 안써도 되겠다.. 나만의 프린스니까 ><",
            "은유만 보면 후광이 보여!!",
            "제발 나를 가져요 엉엉",
            "단신 자체가 내 삶이야"
        });
    }
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

    [SerializeField] private Transform target;
    [SerializeField] private float _minX = -10f;
    [SerializeField] private float _maxX = 10f;
    [SerializeField] private float _pivotY = 4f;

    private bool _isGameEnd = false;

    private Queue<UI_MiniGame> _miniGameQueue = new Queue<UI_MiniGame>();

    public event Action<bool> OnGameEnd;

    public void Init(WorldType worldType)
    {
        SetWorld(worldType);
    
        Managers.Happy.OnHappinessChanged += CreateControll;

        StartCoroutine(CreateMiniGame());
    }

    public IEnumerator CreateMiniGame()
    {
        while (true)
        {
            Debug.Log("CreateMiniGame");
            SpawnInfo spawnInfo = GetRandomPosition();
            UI_MiniGame miniGame = Instantiate(_miniGame, spawnInfo.position, Quaternion.identity);
            MiniGameInfo miniGameInfo = _worldInfo.GetRandomMiniGameInfo();
            _miniGameQueue.Enqueue(miniGame);
            miniGame.Init(miniGameInfo, spawnInfo);
            yield return new WaitForSeconds(4f);
        }
    }

    // target을 기준으로 일정 거리만큼 떨어진 곳에 생성
    private SpawnInfo GetRandomPosition()
    {
        float randomX = UnityEngine.Random.Range(_minX, _maxX);
        bool isLeft = randomX < 0;

        Vector2 spawnPos = target.transform.position;

        if(randomX < 0)
        {
            spawnPos += new Vector2(_minX,  _pivotY);
        }
        else
        {
            spawnPos += new Vector2(_maxX,  _pivotY);
        }

        return new SpawnInfo() { position = spawnPos, isLeft = isLeft };
    }

    // 행복도에 따른 게임종료여부 판단
    private void CreateControll(float happiness)
    {
        if(_isGameEnd) return;

        _isGameEnd = true;

        if (happiness <= 0 || happiness >= 100)
        {
            Debug.Log("GameEnd");
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
        }
    }

    private void OnDestroy()
    {
        Managers.Happy.OnHappinessChanged -= CreateControll;
    }
}
