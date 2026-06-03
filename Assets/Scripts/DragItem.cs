using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public string itemID;

    private Vector3 startPosition;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!enabled) return;

        canvasGroup.blocksRaycasts = false;

        // tampil paling depan
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!enabled) return;

        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void ResetItem()
{
    if (rectTransform == null)
        rectTransform = GetComponent<RectTransform>();

    if (canvasGroup == null)
        canvasGroup = GetComponent<CanvasGroup>();

    enabled = true;

    if (rectTransform != null)
        rectTransform.position = startPosition;

    if (canvasGroup != null)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1f;
    }
}
}