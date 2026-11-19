using UnityEngine;
using UnityEngine.UI;

// 10 Credits. Starts at the top every time, and the sections fade up in order.
public class CreditsScreen : UIScreen
{
    [SerializeField] ScrollRect scroll;
    [SerializeField] CanvasGroup[] sections;

    public override void OnShow()
    {
        scroll.verticalNormalizedPosition = 1f;
        for (int i = 0; i < sections.Length; i++)
            Tween.Fade(this, sections[i], 0f, 1f, 0.4f, Ease.OutQuad, 0.15f + 0.08f * i);
    }
}
