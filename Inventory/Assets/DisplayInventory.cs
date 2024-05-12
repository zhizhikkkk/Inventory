using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;

    Dictionary<InventorySlot,GameObject > itemsDisplayed = new Dictionary<InventorySlot,GameObject>();


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
        for (int i = 0; i < inventory.container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.container[i]))
            {
                itemsDisplayed[inventory.container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
            }
            else
            {
                CreateObject(i);
                
            }
        }
    }
    public void CreateDisplay()
    {
        for(int i = 0; i < inventory.container.Count; i++)
        {
            CreateObject(i);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3((X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN))),(Y_START+( -Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN))),0f);
    }

    public void CreateObject(int i)
    {
        var obj = Instantiate(inventory.container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);

        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
        itemsDisplayed.Add(inventory.container[i], obj);
    }
}