using UnityEngine;
using UnityEngine.UI;

// One screen. Pages fill the screen and slide; modals pop up over the page under them.
[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    public enum Kind { Page, Modal }

    [SerializeField] Kind kind = Kind.Page;
    [SerializeField] RectTransform card;            // a modal's card, the part that pops
    [SerializeField] Selectable firstSelected;      // for keyboard and gamepad

    public Kind ScreenKind => kind;
    public RectTransform Card => card;
    public Selectable FirstSelected => firstSelected;
    public CanvasGroup Group => GetComponent<CanvasGroup>();
    public RectTransform Rect => (RectTransform)transform;

    public virtual void OnShow() { }                 // refresh data, start entrance motion
    public virtual void OnHide(bool closed) { }      // closed = popped off, not just covered
    public virtual void OnBackPressed() => UIRouter.Instance.Back();
}
