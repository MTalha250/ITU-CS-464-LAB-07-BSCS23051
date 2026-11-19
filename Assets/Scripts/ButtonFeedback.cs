using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Press feedback: the button sinks a little under the finger, then springs back with OutBack.
public class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] float pressedScale = 0.94f;
    Selectable selectable;
    Coroutine running;
    bool down;

    void Awake() => selectable = GetComponent<Selectable>();

    void OnDisable()
    {
        down = false;
        transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (selectable && !selectable.interactable) return;
        down = true;
        To(pressedScale, 0.08f, Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData e) => Release();
    public void OnPointerExit(PointerEventData e) => Release();

    void Release()
    {
        if (!down) return;
        down = false;
        To(1f, 0.25f, Ease.OutBack);
    }

    void To(float scale, float duration, System.Func<float, float> ease)
    {
        if (!isActiveAndEnabled) return;
        if (running != null) StopCoroutine(running);
        running = Tween.Scale(this, transform, transform.localScale.x, scale, duration, ease);
    }
}
