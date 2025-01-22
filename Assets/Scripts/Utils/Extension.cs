using System;
using System.Collections;
using System.Collections.Generic;
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
}
