using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        List<InventorySlot> keys = new List<InventorySlot>(itemsDisplayed.Keys);

        foreach (var slot in keys)
        {
            if (inventory.container.Items.Contains(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            else
            {
                Destroy(itemsDisplayed[slot]);
                itemsDisplayed.Remove(slot);
            }
        }

        foreach (var slot in inventory.container.Items)
        {
            if (!itemsDisplayed.ContainsKey(slot))
            {
                CreateItemDisplay(slot, inventory.container.Items.IndexOf(slot));
            }
        }
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.container.Items.Count; i++)
        {
            InventorySlot slot = inventory.container.Items[i];
            CreateItemDisplay(slot, i);
        }
    }

    private void CreateItemDisplay(InventorySlot slot, int index)
    {
        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
        obj.GetComponent<RectTransform>().localPosition = GetPosition(index);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

        var draggableItem = obj.GetComponent<DraggableItem>();
        if (draggableItem == null)
        {
            draggableItem = obj.AddComponent<DraggableItem>();
        }
        draggableItem.Initialize(slot, this);

        var dropZone = obj.GetComponent<DropZone>();
        if (dropZone == null)
        {
            dropZone = obj.AddComponent<DropZone>();
        }
        dropZone.Initialize(this, index);

        itemsDisplayed.Add(slot, obj);
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3((X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN))), (Y_START + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN))), 0f);
    }

    public void MoveItem(InventorySlot fromSlot, int toIndex)
    {
        int fromIndex = inventory.container.Items.IndexOf(fromSlot);
        if (fromIndex < 0 || toIndex < 0 || fromIndex >= inventory.container.Items.Count || toIndex >= inventory.container.Items.Count)
        {
            Debug.Log("Invalid index");
            return;
        }

        InventorySlot temp = inventory.container.Items[toIndex];
        inventory.container.Items[toIndex] = inventory.container.Items[fromIndex];
        inventory.container.Items[fromIndex] = temp;

        UpdateDisplay();
    }
}
