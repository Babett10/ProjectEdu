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

        startPosition = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        // Biar di layer paling atas
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void ResetPosition()
    {
        rectTransform.position = startPosition;
    }
}