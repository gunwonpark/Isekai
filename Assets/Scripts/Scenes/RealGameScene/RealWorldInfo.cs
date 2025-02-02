using System.Collections.Generic;

public abstract class RealWorldInfo 
{
    public List<string> dialog;
    public List<int> score;
}

public class FirstWorldInfo : RealWorldInfo
{
    public FirstWorldInfo()
    {
        dialog = new List<string>()
        {
            "평범하기 짝이 없으면서 어딜 나서려고 하는 거야?",
            "넌 진짜 존재감이라고는 1도 없다.",
            "외모도 능력도 특별할 것 없는 사람에게 뭘 기대하겠어.",
            "누가 너한테 관심이나 있을 줄 알아?",
            "넌 애초에 부족한 사람이야.",
            "너는 대체 뭘 잘하니?",
        };

        score = new List<int>()
        {
            -20, -20, -20, -20, -30, -40
        };
    }
}

public class SecondWorldInfo : RealWorldInfo
{
    public SecondWorldInfo()
    {
        dialog = new List<string>()
        {
            "너 진짜 사람 피곤하게 만든다.",
            "왜 그 청도도 못하니?",
            "진짜 너랑 대화가 하나도 안 된다.",
            "년 왜 자기주장만 해?",
            "너 같은 사람이 무슨 성공을 하겠어?",
            "넌 겔국 혼자 남을거야."
        };

        score = new List<int>()
        {
            -20, -20, -20, -30, -40, -40
        };
    }
}

public class ThirdWorldInfo : RealWorldInfo
{
    public ThirdWorldInfo()
    {
        dialog = new List<string>()
        {
            "그 직업으로는 돈 많이 못 별지 않아?",
            "진짜 아는 게 뭬야?",
            "너네 가족들은 너한테 기대 많이 하셨을 텐데…",
            "또 실패했어?",
            "이것도 못 해?",
            "다 잘 되라고 하는 소리야.",
            "시간 낭비야 그거",
            "언제쯤 남들처럼 살거야?",
            "너한테 너무 큼 기대였구나",
            "언제까지 힘들래?"
        };

        score = new List<int>()
        {
            -10, -10, -20, -20, -20, -20, -20, -20, -30, -30
        };
    }
}
