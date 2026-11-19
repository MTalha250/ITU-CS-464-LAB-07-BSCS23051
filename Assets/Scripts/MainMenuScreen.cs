using TMPro;
using UnityEngine;

// 02 Main menu. The buttons rise in one after another, Play first, so the order you
// see them in is the order of importance.
public class MainMenuScreen : UIScreen
{
    [SerializeField] TMP_Text coins, nextLevel;
    [SerializeField] RectTransform[] items;
    Vector2[] rest;

    public override void OnShow()
    {
        coins.text = GameData.Coins.ToString("N0");
        nextLevel.text = "Next: " + GameData.LevelTitle(Mathf.Min(GameData.Level, GameData.Unlocked));
        if (rest == null)
        {
            rest = new Vector2[items.Length];
            for (int i = 0; i < items.Length; i++) rest[i] = items[i].anchoredPosition;
        }
        for (int i = 0; i < items.Length; i++)
        {
            RectTransform item = items[i];
            Vector2 at = rest[i];
            CanvasGroup g = item.GetComponent<CanvasGroup>();
            Tween.Run(this, 0.45f, Ease.OutCubic, p =>
            {
                item.anchoredPosition = at + Vector2.down * 70f * (1f - p);
                g.alpha = p;
            }, 0.15f + 0.07f * i);
        }
    }
}
