using UnityEngine;

// Lecture 05's easing curves. Each one reshapes linear progress t (0 to 1).
public static class Ease
{
    public static float Linear(float t) => t;
    public static float OutQuad(float t) => 1f - (1f - t) * (1f - t);
    public static float OutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    public static float InCubic(float t) => t * t * t;
    public static float InOutCubic(float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    public static float InOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;

    public static float OutBack(float t)
    {
        const float c1 = 1.70158f, c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}
