using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 시간에 따라 게이지가 점점 줄어든다
/// 스페이스바를 누르면 게이지가 조금씩 찬다
/// 게임지를 100까지 해우면 행복도 상승 그렇지 않으면 행복도 하락
/// 
/// 난이도는 하 중 상으로 나누어져 있다
/// 하 :
///     일반적인 상황
///     
/// 중 :
///     키보드 키를 모두 누른다음 스페리스바를 눌러야 게이지가 찬다 ( 3 ~ 6개 랜덤)
///     
/// 상 :
///     동시에 눌러야 되는 키보드도 등장한다
/// </summary>


#region Data
struct MiniGameInfo
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

public class UI_MiniGame : UI_Popup
{
    [SerializeField] private Image _minigameGaugeBar;
    [SerializeField] private WorldInfo _worldInfo;
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private Transform _miniGamePosition;
    [SerializeField] private Transform[] _miniGameSpawnPosition;
    [SerializeField] private TextMeshProUGUI _remainTimeText;

    [SerializeField] private float _createDelayTime = 4f;

    private float _sudoGauge;
    public float SudoGauge
    {
        get => _sudoGauge;
        set
        {
            _sudoGauge = Mathf.Clamp(value, 0f, 100f); // 게이지 제한 (0 ~ 100)
            _minigameGaugeBar.fillAmount = _sudoGauge / 100f;
        }
    }

    public void Init(WorldInfo worldInfo)
    {
        _worldInfo = worldInfo;
        SudoGauge = 100f;

        StartCoroutine(StartMiniGames());
    }

    private IEnumerator StartMiniGames()
    {
        // 랜덤 인덱스 생성
        List<int> randomIndex = new List<int>(_worldInfo.dialog.Count);
        for (int i = 0; i < _worldInfo.dialog.Count; i++)
        {
            randomIndex.Add(i);
        }
        randomIndex.Shuffle();

        int randomIndexCount = 0;

        // 난이도 순차 실행
        for (int i = 0; i < (int)MiniGameDifficulty.Max; i++)
        {
            for (int j = 0; j < _worldInfo.difficulty[i]; j++)
            {
                if (randomIndexCount >= randomIndex.Count) break;

                MiniGameInfo miniGameInfo = new MiniGameInfo()
                {
                    difficulty = (MiniGameDifficulty)i,
                    startGauge = _worldInfo.startGauge[i],
                    perDecreaseGauge = _worldInfo.perDecreaseGauge[i],
                    perIncreaseGauge = _worldInfo.perIncreaseGauge[i],
                    succedGauge = _worldInfo.succedGauge[i],
                    failGauge = _worldInfo.failGauge[i],
                    runGauge = _worldInfo.runGauge[i],
                    limitTime = _worldInfo.limitTime[i],

                    dialog = _worldInfo.dialog[randomIndex[randomIndexCount++]]

                };

                // key세팅
                if(_worldInfo is ChaummWorldInfo chaummWorldInfo)
                {
                    miniGameInfo.requiredKeys = chaummWorldInfo.requireKeys;
                    miniGameInfo.requiredKeyCount = chaummWorldInfo.requiredKeyCount;
                    miniGameInfo.canPressConcurrent = chaummWorldInfo.canPressConcurrent;
                }
              

                yield return StartCoroutine(HandleMiniGame(miniGameInfo));
                yield return new WaitForSeconds(_createDelayTime);
                _miniGamePosition.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator HandleMiniGame(MiniGameInfo miniGameInfo)
    {
        _miniGamePosition.gameObject.SetActive(true);
        Debug.Log($"미니게임 시작! {miniGameInfo.dialog}");

        // 미니게임 초기화
        SudoGauge = miniGameInfo.startGauge;
        SetMiniGamePositionAndDialog(miniGameInfo);

        List<KeyCode> requiredKeys = null;
        bool canPressConcurrent = false;

        if (miniGameInfo.requiredKeys != null)
        {
            canPressConcurrent = miniGameInfo.canPressConcurrent[(int)miniGameInfo.difficulty];
            int requiredKeyCount = miniGameInfo.requiredKeyCount[(int)miniGameInfo.difficulty];
            requiredKeys = miniGameInfo.requiredKeys.GetRandomN(requiredKeyCount);
        }

        float remainingTime = miniGameInfo.limitTime;
        _remainTimeText.text = $"남은 시간: {remainingTime:F1}초";

        Coroutine decreaseGaugeRoutine = StartCoroutine(DecreaseGauge(miniGameInfo.perDecreaseGauge));
        Coroutine miniGameRoutine = StartCoroutine(MiniGameStart(miniGameInfo, requiredKeys, canPressConcurrent));

        float elapsedTime = 0f;
        while (elapsedTime < miniGameInfo.limitTime)
        {
            _remainTimeText.text = $"남은 시간: {remainingTime - elapsedTime:F1}초";
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        _remainTimeText.text = $"남은 시간: 0.0초";

        // 코루틴 정지
        StopCoroutine(decreaseGaugeRoutine);
        StopCoroutine(miniGameRoutine);

        // 결과 체크
        CheckMiniGameResult(miniGameInfo);
    }

    private void CheckMiniGameResult(MiniGameInfo miniGameInfo)
    {
        if (SudoGauge >= 100)
        {
            // 성공
            Managers.Happy.ChangeHappiness(miniGameInfo.succedGauge);
            Debug.Log("미니게임 성공!");
        }
        else
        {
            // 실패
            Managers.Happy.ChangeHappiness(miniGameInfo.failGauge);
            Debug.Log("미니게임 실패...");
        }
    }

    private void SetMiniGamePositionAndDialog(MiniGameInfo miniGameInfo)
    {
        int pos = UnityEngine.Random.Range(0, _miniGameSpawnPosition.Length);
        _miniGamePosition.position = _miniGameSpawnPosition[pos].position;
        _dialogText.text = miniGameInfo.dialog;
    }

    private IEnumerator MiniGameStart(MiniGameInfo miniGameInfo, List<KeyCode> keys, bool canPressConcurrent)
    {
        
        while (true)
        {
            if (miniGameInfo.difficulty == MiniGameDifficulty.Easy)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SudoGauge += miniGameInfo.perIncreaseGauge;
                }
            }
            else if (miniGameInfo.difficulty == MiniGameDifficulty.Normal)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SudoGauge += miniGameInfo.perIncreaseGauge;
                }
            }
            else if (miniGameInfo.difficulty == MiniGameDifficulty.Hard)
            {
                
            }

            yield return null;
        }
    }

    // 1초마다 게이지 감소
    private IEnumerator DecreaseGauge(int perDecreaseGauge)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SudoGauge += perDecreaseGauge;
        }
    }

    private void SpawnKeys(List<KeyCode> keys, bool canPressConcurrent)
    {
        RectTransform rectTransform = _minigameGaugeBar.rectTransform;
        float startX = rectTransform.position.x - rectTransform.rect.width / 2;
        float endX = rectTransform.position.x + rectTransform.rect.width / 2;

        for (int i = 0; i < keys.Count; i++)
        {
            float interval = (i + 1) / (keys.Count + 1);
            float posX = Mathf.Lerp(startX, endX, interval);
            float posY = rectTransform.position.y;

            Managers.UI.MakeSubItem<KeyButton>(this.transform);
        }
    }

    public override void Init()
    {
        // 추가 초기화가 필요한 경우 여기에 작성
    }
}