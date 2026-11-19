using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 06 Settings. Every change applies at once and is saved (PlayerPrefs), so there's no Save button to miss.
public class SettingsScreen : UIScreen
{
    [SerializeField] Slider music, sfx;
    [SerializeField] TMP_Text musicValue, sfxValue;
    [SerializeField] Toggle vibration, reduceMotion;
    bool wired;

    public override void OnShow()
    {
        if (wired) return;
        wired = true;
        music.SetValueWithoutNotify(PlayerPrefs.GetFloat("music", 0.8f));
        sfx.SetValueWithoutNotify(PlayerPrefs.GetFloat("sfx", 1f));
        vibration.SetIsOnWithoutNotify(PlayerPrefs.GetInt("vibration", 1) == 1);
        reduceMotion.SetIsOnWithoutNotify(PlayerPrefs.GetInt("reduceMotion", 0) == 1);
        ShowValues();
        vibration.GetComponent<ToggleSwitch>().Sync();
        reduceMotion.GetComponent<ToggleSwitch>().Sync();
        music.onValueChanged.AddListener(v => { PlayerPrefs.SetFloat("music", v); ShowValues(); });
        sfx.onValueChanged.AddListener(v => { PlayerPrefs.SetFloat("sfx", v); ShowValues(); });
        vibration.onValueChanged.AddListener(v => PlayerPrefs.SetInt("vibration", v ? 1 : 0));
        reduceMotion.onValueChanged.AddListener(v =>
        {
            PlayerPrefs.SetInt("reduceMotion", v ? 1 : 0);
            UIRouter.ReduceMotion = v;          // fades instead of slides, and shorter tweens
            Tween.Speed = v ? 0.6f : 1f;
        });
    }

    void ShowValues()
    {
        musicValue.text = Mathf.RoundToInt(music.value * 100f) + "%";
        sfxValue.text = Mathf.RoundToInt(sfx.value * 100f) + "%";
    }
}
