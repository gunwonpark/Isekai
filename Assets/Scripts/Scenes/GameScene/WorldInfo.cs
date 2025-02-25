using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldInfo
{
    public WorldType worldType;

    public List<int> difficulty = new List<int>(); // 0: Easy, 1: Normal, 2: Hard
    public List<int> startGauge = new List<int>(); // 시작 게이지
    public List<int> perDecreaseGauge = new List<int>(); // 게이지 감소량
    public List<int> perIncreaseGauge = new List<int>(); // 게이지 증가량
    public List<int> succedGauge = new List<int>(); // 성공시 증가 게이지
    public List<int> failGauge = new List<int>(); // 실패시 감소 게이지
    public List<int> runGauge = new List<int>(); // ???
    public List<int> limitTime = new List<int>(); // 제한 시간

    public List<string> dialog = new List<string>(); // 대사

    public List<KeyCode> requireKeys = new List<KeyCode>(); // 가지고 있는 키 정도
    public List<int> requiredKeyCount = new List<int>(); // 필요한 키 총 개수
    public List<bool> canPressConcurrent = new List<bool>(); // 동시에 누를 수 있는지 여부

    public WorldInfo(WorldType worldType)
    {
        this.worldType = worldType;
    }

    public WorldInfo(GameSceneData gameSceneData)
    {
        worldType = gameSceneData.worldType;
        difficulty = gameSceneData.difficulty;
        startGauge = gameSceneData.startGauge;
        perDecreaseGauge = gameSceneData.perDecreaseGauge;
        perIncreaseGauge = gameSceneData.perIncreaseGauge;
        succedGauge = gameSceneData.succedGauge;
        failGauge = gameSceneData.failGauge;
        runGauge = gameSceneData.runGauge;
        limitTime = gameSceneData.limitTime;
        dialog = gameSceneData.dialog;
        requireKeys = gameSceneData.requireKeys;
        requiredKeyCount = gameSceneData.requiredKeyCount;
        canPressConcurrent = gameSceneData.canPressConcurrent;
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

[System.Serializable]
public class VinterWorldInfo : WorldInfo
{
    public VinterWorldInfo(GameSceneData gameSceneData) : base(gameSceneData)
    {
    }
}

[System.Serializable]
public class ChaummWorldInfo : WorldInfo
{
    public ChaummWorldInfo(GameSceneData gameSceneData) : base(gameSceneData)
    {
    }
    public override MiniGameInfo GetRandomMiniGameInfo()
    {
        MiniGameInfo miniGameInfo = base.GetRandomMiniGameInfo();

        int keyCount = requiredKeyCount[(int)miniGameInfo.difficulty];

        if (keyCount != 0)
        {
            miniGameInfo.requiredKeys = requireKeys.GetRandomN<KeyCode>(keyCount);
            miniGameInfo.requiredKeyCount = keyCount;
            miniGameInfo.canPressConcurrent = canPressConcurrent[(int)miniGameInfo.difficulty];
        }

        return miniGameInfo;
    }
}

[System.Serializable]
public class GangWorldInfo : WorldInfo
{
    public GangWorldInfo(GameSceneData gameSceneData) : base(gameSceneData)
    {
    }
    public override MiniGameInfo GetRandomMiniGameInfo()
    {
        MiniGameInfo miniGameInfo = base.GetRandomMiniGameInfo();

        int keyCount = requiredKeyCount[(int)miniGameInfo.difficulty];

        if (keyCount != 0)
        {
            miniGameInfo.requiredKeyCount = keyCount;
            miniGameInfo.canPressConcurrent = canPressConcurrent[(int)miniGameInfo.difficulty];

            if(miniGameInfo.canPressConcurrent)
            {
                miniGameInfo.requiredKeys = requireKeys.GetRandomN<KeyCode>(keyCount + 2);
            }
            else
            {
                miniGameInfo.requiredKeys = requireKeys.GetRandomN<KeyCode>(keyCount);
            }
        }

        return miniGameInfo;
    }
}

[System.Serializable]
public class PelmanusWorldInfo : WorldInfo
{
    public PelmanusWorldInfo(GameSceneData gameSceneData) : base(gameSceneData)
    {
    }
    public override MiniGameInfo GetRandomMiniGameInfo()
    {
        MiniGameInfo miniGameInfo = base.GetRandomMiniGameInfo();

        int keyCount = requiredKeyCount[(int)miniGameInfo.difficulty];

        if (keyCount != 0)
        {
            miniGameInfo.requiredKeyCount = keyCount;
            miniGameInfo.canPressConcurrent = canPressConcurrent[(int)miniGameInfo.difficulty];
            miniGameInfo.requiredKeys = requireKeys.GetRandomN<KeyCode>(keyCount);
        }

        return miniGameInfo;
    }
}