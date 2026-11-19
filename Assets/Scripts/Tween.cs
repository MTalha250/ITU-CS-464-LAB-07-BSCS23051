using System;
using System.Collections;
using UnityEngine;

// Lecture 05's tween coroutine, made reusable. It runs on unscaled time,
// so menus still animate while the game is paused (Time.timeScale = 0).
public static class Tween
{
    public static float Speed = 1f;   // Reduce Motion makes every tween shorter

    public static Coroutine Run(MonoBehaviour host, float duration, Func<float, float> ease, Action<float> step, float delay = 0f) =>
        host.StartCoroutine(Routine(duration, ease, step, delay));

    static IEnumerator Routine(float duration, Func<float, float> ease, Action<float> step, float delay)
    {
        step(0f);
        if (delay > 0f) yield return new WaitForSecondsRealtime(delay * Speed);
        duration *= Speed;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            step(ease(t / duration));
            yield return null;
        }
        step(1f);   // land exactly on the end
    }

    public static Coroutine Move(MonoBehaviour host, RectTransform rt, Vector2 from, Vector2 to, float duration, Func<float, float> ease, float delay = 0f) =>
        Run(host, duration, ease, p => rt.anchoredPosition = Vector2.LerpUnclamped(from, to, p), delay);

    public static Coroutine Scale(MonoBehaviour host, Transform t, float from, float to, float duration, Func<float, float> ease, float delay = 0f) =>
        Run(host, duration, ease, p => t.localScale = Vector3.one * Mathf.LerpUnclamped(from, to, p), delay);

    public static Coroutine Fade(MonoBehaviour host, CanvasGroup g, float from, float to, float duration, Func<float, float> ease, float delay = 0f) =>
        Run(host, duration, ease, p => g.alpha = Mathf.LerpUnclamped(from, to, p), delay);

    // A "no" shake: a sine wave that dies away. It reads as an error without any text.
    public static Coroutine Shake(MonoBehaviour host, RectTransform rt, float strength = 28f, float duration = 0.4f)
    {
        Vector2 rest = rt.anchoredPosition;
        return Run(host, duration, Ease.Linear, p =>
            rt.anchoredPosition = rest + Vector2.right * Mathf.Sin(p * Mathf.PI * 6f) * strength * (1f - p));
    }
}
