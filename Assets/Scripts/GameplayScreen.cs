using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 07 Gameplay (a small stand-in level). Steer the lantern with A/D, the arrows, or by touching
// the river, and catch the sparks. The bar at the top shows how far down the river you are.
public class GameplayScreen : UIScreen
{
    [SerializeField] RectTransform field, lantern, sparkTemplate;
    [SerializeField] Image progress, lanternBody;
    [SerializeField] TMP_Text sparksText, levelText;
    [SerializeField] RectTransform sparksChip;
    [SerializeField] UIScreen pause, results;
    [SerializeField] float length = 15f, sparkEvery = 0.6f, fallSpeed = 620f, steerSpeed = 1100f;

    readonly List<RectTransform> live = new List<RectTransform>();
    InputAction move;
    float elapsed, nextSpark;
    int sparks;
    bool running;

    public override void OnShow()
    {
        Time.timeScale = 1f;
        if (running) return;              // back from Pause: carry on where we were
        running = true;
        move ??= InputSystem.actions.FindAction("Player/Move");
        foreach (var s in live) Destroy(s.gameObject);
        live.Clear();
        elapsed = 0f;
        nextSpark = 0.5f;
        sparks = 0;
        sparksText.text = "0";
        progress.fillAmount = 0f;
        levelText.text = GameData.LevelTitle(GameData.Level);
        lanternBody.color = GameData.LanternColors[GameData.Equipped];
        lantern.anchoredPosition = new Vector2(0f, lantern.anchoredPosition.y);
    }

    public override void OnHide(bool closed)
    {
        if (closed) running = false;
    }

    public override void OnBackPressed() => UIRouter.Instance.Go(pause);
    public void Pause() => UIRouter.Instance.Go(pause);

    void Update()
    {
        if (!running || UIRouter.Instance.Current != this || UIRouter.Instance.Busy) return;
        float dt = Time.deltaTime;
        elapsed += dt;
        progress.fillAmount = elapsed / length;
        Steer(dt);

        nextSpark -= dt;
        if (nextSpark <= 0f)
        {
            nextSpark = sparkEvery;
            var s = Instantiate(sparkTemplate, field);
            s.gameObject.SetActive(true);
            s.anchoredPosition = new Vector2(Random.Range(-380f, 380f), field.rect.height * 0.5f + 60f);
            live.Add(s);
        }
        for (int i = live.Count - 1; i >= 0; i--)
        {
            var s = live[i];
            s.anchoredPosition += Vector2.down * fallSpeed * dt;
            Vector2 d = s.anchoredPosition - lantern.anchoredPosition;
            if (Mathf.Abs(d.x) < 110f && Mathf.Abs(d.y) < 110f)
            {
                sparks++;
                sparksText.text = sparks.ToString();
                Tween.Scale(this, sparksChip, 1.25f, 1f, 0.25f, Ease.OutBack);   // the counter answers the catch
                Destroy(s.gameObject);
                live.RemoveAt(i);
            }
            else if (s.anchoredPosition.y < -field.rect.height * 0.5f - 60f)
            {
                Destroy(s.gameObject);
                live.RemoveAt(i);
            }
        }

        if (elapsed >= length)
        {
            running = false;
            GameData.Finish(sparks);
            UIRouter.Instance.Replace(results);
        }
    }

    void Steer(float dt)
    {
        float x = lantern.anchoredPosition.x;
        float input = move != null ? move.ReadValue<Vector2>().x : 0f;
        x += input * steerSpeed * dt;
        var pointer = Pointer.current;
        if (pointer != null && pointer.press.isPressed &&
            RectTransformUtility.ScreenPointToLocalPointInRectangle(field, pointer.position.ReadValue(), GetComponentInParent<Canvas>().worldCamera, out Vector2 local))
            x = Mathf.MoveTowards(x, local.x, steerSpeed * dt);
        lantern.anchoredPosition = new Vector2(Mathf.Clamp(x, -420f, 420f), lantern.anchoredPosition.y);
    }
}
