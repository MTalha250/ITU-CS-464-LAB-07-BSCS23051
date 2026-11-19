using UnityEngine;

// Keeps the UI inside the phone's safe area (away from notches and the home bar).
[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    Rect applied;

    void Update()
    {
        Rect safe = Screen.safeArea;
        if (safe == applied || Screen.width == 0) return;
        applied = safe;
        var rt = (RectTransform)transform;
        rt.anchorMin = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
        rt.anchorMax = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
    }
}
