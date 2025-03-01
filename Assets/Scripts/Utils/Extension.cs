using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class WaitForSecondsCache
{
    private static Dictionary<float, WaitForSeconds> cache = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds Get(float time)
    {
        if (!cache.TryGetValue(time, out var wait))
        {
            wait = new WaitForSeconds(time);
            cache[time] = wait;
        }
        return wait;
    }
}


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

    public static IEnumerator CoFadeIn(this Graphic graphic, float fadeTime, float waitBefore = 0f, float waitAfter = 0f)
    {
        if (waitBefore > 0f) yield return WaitForSecondsCache.Get(waitBefore);

        Color color = graphic.color;
        float startAlpha = 1;
        float targetAlpha = 0;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeTime);
            graphic.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        graphic.color = color;

        if (waitAfter > 0f) yield return WaitForSecondsCache.Get(waitAfter);
    }
    public static IEnumerator CoFadeOut(this Graphic graphic, float fadeTime, float waitBefore = 0f, float waitAfter = 0f)
    {
        if (waitBefore > 0f) yield return WaitForSecondsCache.Get(waitBefore);

        Color color = graphic.color;
        float startAlpha = 0;
        float targetAlpha = 1;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeTime);
            graphic.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        graphic.color = color;

        if (waitAfter > 0f) yield return WaitForSecondsCache.Get(waitAfter);
    }

    public static IEnumerator CoFadeIn(this SpriteRenderer sprite, float fadeTime, float waitBefore = 0f, float waitAfter = 0f)
    {
        if (waitBefore > 0f) yield return WaitForSecondsCache.Get(waitBefore);

        Color color = sprite.color;
        float startAlpha = 1;
        float targetAlpha = 0;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeTime);
            sprite.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        sprite.color = color;

        if (waitAfter > 0f) yield return WaitForSecondsCache.Get(waitAfter);
    }
    public static IEnumerator CoFadeOut(this SpriteRenderer sprite, float fadeTime, float waitBefore = 0f, float waitAfter = 0f)
    {
        if (waitBefore > 0f) yield return WaitForSecondsCache.Get(waitBefore);

        Color color = sprite.color;
        float startAlpha = 0;
        float targetAlpha = 1;
        float elapsedTime = 0;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeTime);
            sprite.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        sprite.color = color;

        if (waitAfter > 0f) yield return WaitForSecondsCache.Get(waitAfter);
    }


    public static IEnumerator CoFillImage(this Image image, float targetFill, float duration)
    {
        float startFill = image.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / duration);
            yield return null;
        }

        image.fillAmount = targetFill;
    }

    public static IEnumerator CoBlinkText(this TMP_Text text, int blinkCount, float blinkDuration)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            yield return CoFadeIn(text, blinkDuration / 2);
            yield return CoFadeOut(text, blinkDuration / 2);
        }
    }

    public static IEnumerator CoTypingEffect(this TMP_Text text, string message, float typingSpeed)
    {
        text.text = "";
        foreach (char letter in message)
        {
            text.text += letter;
            yield return WaitForSecondsCache.Get(typingSpeed);
        }
    }

    public static string GetRandomMaskedText(int length, string maskCharacters = "#*@$%&!")
    {
        StringBuilder randomText = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            randomText.Append(maskCharacters[UnityEngine.Random.Range(0, maskCharacters.Length)]);
        }
        return randomText.ToString();
    }

    public static string GetRandomMaskedText(this string text, string maskCharacters = "#*@$%&!")
    {
        return GetRandomMaskedText(text.Length, maskCharacters);
    }

    public static string GetNRandomMaskedText(this string text, int n, string maskCharacters = "#*@$%&!")
    {
        StringBuilder stringBuilder = new StringBuilder(text);
        List<int> randomIndices = Enumerable.Range(0, text.Length).ToList();
        randomIndices.Shuffle();

        foreach (int index in randomIndices)
        {
            stringBuilder[index] = maskCharacters[UnityEngine.Random.Range(0, maskCharacters.Length)];
        }
        return stringBuilder.ToString();
    }
}
