using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(go);
	}

	public static void BindEvent(this GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
	{
		UI_Base.BindEvent(go, action, type);
	}

	public static bool IsValid(this GameObject go)
	{
		return go != null && go.activeSelf;
	}

	public static IEnumerator CoTypingAndWait(this TMPro.TextMeshProUGUI text, string targetString, float duration)
    {
		foreach(var c in targetString)
		{
			text.text += c;
            yield return new WaitForSeconds(duration);
        }
    }

    public static IEnumerator CoTypingAndWait(this TMPro.TMP_Text text, string targetString, float duration)
    {
        foreach (var c in targetString)
        {
            text.text += c;
            yield return new WaitForSeconds(duration);
        }
    }

	public static void Shuffle<T>(this List<T> list)
	{
		System.Random rand = new System.Random();

		for(int i = list.Count - 1; i > 0; i--)
		{
			int randIdx = rand.Next(i + 1);
			(list[i], list[randIdx]) = (list[randIdx], list[i]);
        }
	}

	public static List<T> GetRandomN<T>(this List<T> list, int n)
	{
		List<T> result = new List<T>(list);
        result.Shuffle();
        return result.GetRange(0, n);
    }

    // 시간적으로는 거의 n이 걸리지만 복잡한 문장에는 오류가 있을 수 있다
    // ex '>' '<' 이 문자가 너무 무분별하거나 복잡하게 포합되어 있는 경우
    public static string RemoveRichTextTags(this string input)
    {
        // <.*?> : <로 시작하고 >로 끝나는 모든 문자열
        return Regex.Replace(input, "<.*?>", string.Empty);
    }

    public static IEnumerator CoFadeOut(this UnityEngine.UI.Image image, float fadeTime, float waitTimeAfterfade = 0f)
    {
        Color color = image.color;
        float curTime = 0;

        while (curTime < fadeTime)
        {
            curTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, curTime / fadeTime);
            image.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(waitTimeAfterfade);
    }
}
