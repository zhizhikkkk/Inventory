using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public DisplayInventory displayInventory;
    public int slotIndex;

    public void Initialize(DisplayInventory displayInventory, int slotIndex)
    {
        this.displayInventory = displayInventory;
        this.slotIndex = slotIndex;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggableItem != null)
        {
            displayInventory.MoveItem(draggableItem.slot, slotIndex);

            // Перемещаем объект в новую позицию
            draggableItem.transform.SetParent(transform);
            draggableItem.transform.localPosition = Vector3.zero;
        }
    }
}