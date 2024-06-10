using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot slot;
    private Transform originalParent;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private DisplayInventory displayInventory;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            Debug.LogWarning("CanvasGroup was missing, so it was added.");
        }

        canvas = FindCanvasInParents();
        if (canvas == null)
        {
            Debug.LogError("Canvas is not found in parent hierarchy!");
        }

        displayInventory = FindObjectOfType<DisplayInventory>();
        if (displayInventory == null)
        {
            Debug.LogError("DisplayInventory component is not found in the scene!");
        }
    }

    private Canvas FindCanvasInParents()
    {
        Transform currentTransform = transform;
        while (currentTransform != null)
        {
            Canvas foundCanvas = currentTransform.GetComponent<Canvas>();
            if (foundCanvas != null)
            {
                return foundCanvas;
            }
            currentTransform = currentTransform.parent;
        }
        return null;
    }

    public void Initialize(InventorySlot slot, DisplayInventory displayInventory)
    {
        this.slot = slot;
        this.displayInventory = displayInventory;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas is not initialized in OnBeginDrag");
            return;
        }
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is not initialized in OnBeginDrag");
            return;
        }

        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null || canvas == null)
        {
            Debug.LogError("RectTransform or Canvas is null in OnDrag");
            return;
        }

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup == null || originalParent == null)
        {
            Debug.LogError("CanvasGroup or originalParent is null in OnEndDrag");
            return;
        }

        canvasGroup.blocksRaycasts = true;

        bool droppedOnValidZone = false;

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            var dropZone = result.gameObject.GetComponent<DropZone>();
            if (dropZone != null)
            {
                dropZone.OnDrop(eventData);
                droppedOnValidZone = true;
                break;
            }
        }

        if (!droppedOnValidZone)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
    }
}
