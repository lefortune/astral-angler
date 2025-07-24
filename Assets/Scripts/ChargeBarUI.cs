using UnityEngine;
using UnityEngine.UI;

public class ChargeBarUI : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform barArea;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePointer(float t)
    {
        t = Mathf.Clamp01(t); // safety

        float barWidth = barArea.rect.width;
        Vector2 newPos = pointer.anchoredPosition;
        newPos.x = -barWidth / 2f + t * barWidth;
        pointer.anchoredPosition = newPos;

        Debug.Log($"Pointer moved to: {newPos.x} (bar width: {barWidth}, t: {t})");
    }
}
