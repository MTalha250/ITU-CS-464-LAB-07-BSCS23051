using System.Collections;
using UnityEngine;

// 01 Title. The logo drops in first (it's the first thing to read), then "Tap to start" pulses
// so it's clear the whole screen is waiting for a tap.
public class TitleScreen : UIScreen
{
    [SerializeField] RectTransform logo;
    [SerializeField] CanvasGroup tagline, tapHint;
    [SerializeField] UIScreen next;
    Vector2 logoRest;
    bool stored;

    public override void OnShow()
    {
        if (!stored) { logoRest = logo.anchoredPosition; stored = true; }
        Tween.Move(this, logo, logoRest + new Vector2(0f, 260f), logoRest, 0.8f, Ease.OutBack);
        Tween.Fade(this, tagline, 0f, 1f, 0.5f, Ease.OutQuad, 0.4f);
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        tapHint.alpha = 0f;
        yield return new WaitForSecondsRealtime(0.9f);
        while (true)
        {
            yield return Tween.Fade(this, tapHint, 0.35f, 1f, 0.9f, Ease.InOutSine);
            yield return Tween.Fade(this, tapHint, 1f, 0.35f, 0.9f, Ease.InOutSine);
        }
    }

    public void Continue() => UIRouter.Instance.Go(next);

    public override void OnBackPressed() { }   // nothing comes before the title
}
