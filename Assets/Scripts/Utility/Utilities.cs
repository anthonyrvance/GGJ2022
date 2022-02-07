using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I made this :)

public static class Utilities
{
    public static IEnumerator Fade(CanvasGroup cv, float startValue, float endValue, float duration)
    {
        float elapsedTime = 0.0f;
        cv.alpha = startValue;

        while (elapsedTime < duration)
        {
            cv.alpha = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cv.alpha = endValue;
    }
}
