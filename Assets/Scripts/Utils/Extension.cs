using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
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

    public static IEnumerator CoFadeOut(this UnityEngine.UI.Image image, float fadeTime, float waitTimeAfterFade = 0f, float waitTimeBeforeFade = 0f)
    {
        if (waitTimeBeforeFade > 0f) yield return new WaitForSeconds(waitTimeBeforeFade);
        Color color = image.color;
        float curTime = 0;

        while (curTime <= fadeTime)
        {
            curTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, curTime / fadeTime);
            image.color = color;
            yield return null;
        }

        if (waitTimeAfterFade > 0f) yield return new WaitForSeconds(waitTimeAfterFade);
    }

    public static IEnumerator CoFadeIn(this UnityEngine.UI.Image image, float fadeTime, float waitTimeAfterFade = 0f, float waitTimeBeforeFade = 0f)
    {
        if(waitTimeBeforeFade > 0f) yield return new WaitForSeconds(waitTimeBeforeFade);
        Color color = image.color;
        float curTime = 0;

        while (curTime <= fadeTime)
        {
            curTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, curTime / fadeTime);
            image.color = color;
            yield return null;
        }

        if(waitTimeAfterFade > 0f) yield return new WaitForSeconds(waitTimeAfterFade);
    }

    public static IEnumerator CoFillImage(this UnityEngine.UI.Image image, float targetFill, float duration)
    {
        float startFill = image.fillAmount;
        float time = 0f;

        while (time <= duration)
        {
            time += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(startFill, targetFill, time / duration);
            yield return null;
        }

        image.fillAmount = targetFill; // 정확한 목표 값 보정
    }

    public static IEnumerator BlinkTipText(this TMP_Text text, float blinkCount, float perTime)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
            yield return new WaitForSeconds(perTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
            yield return new WaitForSeconds(perTime);
        }
    }

    public static IEnumerator CoTypeingEffect(this TMP_Text text, string message, float typingSpeed)
    {
        text.text = "";

        foreach (char letter in message)
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public static IEnumerator CoTypeingEffect(this TextMeshProUGUI text, string message, float typingSpeed)
    {
        text.text = "";

        foreach (char letter in message)
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public static string GetRandomMaskedText(this string text, int length, string maskCharacters = "#*@$%&!")
    {
        StringBuilder randomText = new StringBuilder(length);

        // 랜덤 문자 배열 생성
        for (int i = 0; i < length; i++)
        {
            // maskCharacters 배열에서 랜덤으로 선택된 문자 추가
            randomText.Append(maskCharacters[UnityEngine.Random.Range(0, maskCharacters.Length)]);
        }

        text = randomText.ToString();
        return text;
    }

    public static string GetRandomMaskedText(this string text, string maskCharacters = "#*@$%&!")
    {
        int length = text.Length;
        StringBuilder randomText = new StringBuilder(length);

        // 랜덤 문자 배열 생성
        for (int i = 0; i < length; i++)
        {
            // maskCharacters 배열에서 랜덤으로 선택된 문자 추가
            randomText.Append(maskCharacters[UnityEngine.Random.Range(0, maskCharacters.Length)]);
        }

        return randomText.ToString();
    }

    public static string GetNRandomMaskedText(this string text, int n, string maskCharacters = "#*@$%&!")
    {
        StringBuilder stringBuilder = new StringBuilder(text);
        int length = text.Length;

        List<int> randomIndex = Enumerable.Range(0, length).ToList();

        randomIndex.Shuffle();

        // 랜덤 문자 배열 생성
        for (int i = 0; i < n; i++)
        {
            stringBuilder[randomIndex[i]] = maskCharacters[UnityEngine.Random.Range(0, maskCharacters.Length)];
        }

        return stringBuilder.ToString();
    }
}
