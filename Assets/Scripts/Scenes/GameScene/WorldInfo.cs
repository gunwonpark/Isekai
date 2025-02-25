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

    public readonly List<string> dialog = new List<string>(); // 대사

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
        perDecreaseGauge.AddRange(new List<int>() { -10, -10, -10 });
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

public class GangWorldInfo : WorldInfo
{
    public readonly List<KeyCode> requireKeys = new List<KeyCode>()
    { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D };
    public List<int> requiredKeyCount = new List<int>() { 0, 4, 3 };
    public List<bool> canPressConcurrent = new List<bool>() { false, false, true };
    public GangWorldInfo() : base(WorldType.Chaumm)
    {
        difficulty.AddRange(new List<int>() { 0, 6, 3 });
        startGauge.AddRange(new List<int>() { 0, 50, 40 });
        perDecreaseGauge.AddRange(new List<int>() { 0, -15, -20 });
        perIncreaseGauge.AddRange(new List<int>() { 0, 3, 2 });
        succedGauge.AddRange(new List<int>() { 0, 20, 10 });
        failGauge.AddRange(new List<int>() { 0, -5, -5 });
        runGauge.AddRange(new List<int>() { 0, -10, -10 });
        limitTime.AddRange(new List<int>() { 0, 4, 3 });

        dialog.AddRange(new List<string>
        {
            "너의 지혜와 재치는 그 누구도 따라올 수 없을 거야!",
            "영리함이 너의 가장 큰 매력이야.",
            "너는 사람들 사이에서도 두각을 드러내는 특별한 존재야.",
            "함께 있으면 즐거워 !!",
            "너의 긍정적 에너지가 나에게 많은 힘이 돼.",
            "너는 참 믿음직스러워.",
            "너의 적응력은 정말 대단해.",
            "너는 뭐든지 해낼 수 있는 존재야!",
            "나도 너처럼 되고 싶어"
        });
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

public class PelmanusWorldInfo : WorldInfo
{
    public readonly List<KeyCode> requireKeys = new List<KeyCode>()
    { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.R};
    public List<int> requiredKeyCount = new List<int>() { 0, 4, 3 };
    public List<bool> canPressConcurrent = new List<bool>() { false, false, false };
    public PelmanusWorldInfo() : base(WorldType.Chaumm)
    {
        difficulty.AddRange(new List<int>() { 0, 3, 6 });
        startGauge.AddRange(new List<int>() { 0, 40, 40 });
        perDecreaseGauge.AddRange(new List<int>() { 0, -10, -20 });
        perIncreaseGauge.AddRange(new List<int>() { 0, 3, 3 });
        succedGauge.AddRange(new List<int>() { 0, 1, 1 });
        failGauge.AddRange(new List<int>() { 0, -5, -5 });
        runGauge.AddRange(new List<int>() { 0, -10, -10 });
        limitTime.AddRange(new List<int>() { 0, 4, 3 });

        dialog.AddRange(new List<string>
        {
            "너의 존재만으로도 세상이 축복받은 것 같아.",
            "그는 희망을 불어넣는 빛과 같은 존재야!!",
            "너의 곁에는 항상 평화와 경의심이 가득해.",
            "정말 성스러움이 느껴져.",
            "그의 손길은 기적을 일으켜.",
            "바라보기만 해도 경이로워.",
            "그의 존재는 인간에게 있어 가장 큰 선물이야.",
            "당신을 바라보기만 해도 행복해.",
            "나도 너처럼 행복해지고 싶어."
        });
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