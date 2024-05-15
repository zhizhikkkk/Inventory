using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.ComponentModel;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public  ItemDatabaseObject database;
    public Inventory container;
    


    public void AddItem(Item item, int amount)
    {

        if(item.buffs.Length>0) {
            container.Items.Add(new InventorySlot(item.Id,item,amount));
            return;
        }
       

        for (int i = 0; i < container.Items.Count; i++)
        {
            if (container.Items[i].item.Id == item.Id)
            {
                container.Items[i].AddAmount(amount);
                return;
            }
        }

        container.Items.Add(new InventorySlot(item.Id,item, amount));
        
    }


    [ContextMenu("Save")]
    public void Save()
    {
        //Debug.Log("Save");
        //string saveData = JsonUtility.ToJson(this, true);
        //File.WriteAllText(string.Concat(Application.persistentDataPath, savePath), saveData);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        Debug.Log("Load");
        //string fullPath = string.Concat(Application.persistentDataPath, savePath);
        //if (File.Exists(fullPath))
        //{
        //    string saveData = File.ReadAllText(fullPath);
        //    JsonUtility.FromJsonOverwrite(saveData, this);
        //}
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

    public InventorySlot(int id,Item item, int amount)
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
