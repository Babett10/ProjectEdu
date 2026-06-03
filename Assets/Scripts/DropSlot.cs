using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string correctID;
    public MatchingManager manager;

    public void OnDrop(PointerEventData eventData)
    {
        DragItem item =
            eventData.pointerDrag.GetComponent<DragItem>();

        if (item == null) return;

        // Jawaban benar
        if (item.itemID == correctID)
        {
            item.transform.position = transform.position;

            manager.AddMatch(item.gameObject);

            // Matikan drag setelah benar
            item.enabled = false;
        }
        // Jawaban salah
        else
        {
            item.ResetItem();
        }
    }
}