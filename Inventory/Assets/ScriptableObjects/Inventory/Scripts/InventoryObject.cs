using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.ComponentModel;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory container;

   

    public void AddItem(Item item, int amount)
    {
        Debug.Log($"Adding item {item.Name} with amount {amount}");

        int remainingAmount = amount;

        for (int i = 0; i < container.Items.Count; i++)
        {
            if (container.Items[i].item.Id == item.Id)
            {
                int availableSpace = item.maxStackSize - container.Items[i].amount;
                if (availableSpace > 0)
                {
                    int amountToAdd = Mathf.Min(remainingAmount, availableSpace);
                    container.Items[i].AddAmount(amountToAdd);
                    remainingAmount -= amountToAdd;
                    Debug.Log($"Added {amountToAdd} of item {item.Name} to existing slot. Remaining amount: {remainingAmount}");

                    if (remainingAmount <= 0)
                    {
                        return;
                    }
                }
            }
        }

        while (remainingAmount > 0)
        {
            int amountToAdd = Mathf.Min(remainingAmount, item.maxStackSize);
            container.Items.Add(new InventorySlot(item.Id, item, amountToAdd));
            remainingAmount -= amountToAdd;
            Debug.Log($"Created new slot for item {item.Name}, amount added: {amountToAdd}. Remaining amount: {remainingAmount}");
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
        container = (Inventory)formatter.Deserialize(stream);
        stream.Close();
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int ID;
    public int amount;

    public InventorySlot(int id, Item item, int amount)
    {
        ID = id;
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
