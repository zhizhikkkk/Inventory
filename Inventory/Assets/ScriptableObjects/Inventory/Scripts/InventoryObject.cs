using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject database;
    public List<InventorySlot> container = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = AssetDatabase.LoadAssetAtPath<ItemDatabaseObject>("Assets/Resources/Database.asset");
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }

    public void AddItem(ItemObject item, int amount)
    {
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item == item)
            {
                container[i].AddAmount(amount);
                return;
            }
        }

        container.Add(new InventorySlot(database.GetId[item],item, amount));
        
    }


    
    public void Save()
    {
        Debug.Log("Save");
        string saveData = JsonUtility.ToJson(this, true);
        File.WriteAllText(string.Concat(Application.persistentDataPath, savePath), saveData);
    }

    public void Load()
    {
        Debug.Log("Load");
        string fullPath = string.Concat(Application.persistentDataPath, savePath);
        if (File.Exists(fullPath))
        {
            string saveData = File.ReadAllText(fullPath);
            JsonUtility.FromJsonOverwrite(saveData, this);
        }
    }
    public void OnAfterDeserialize()
    {
        for(int i = 0; i < container.Count; i++)
        {
            container[i].item = database.GetItem[container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}


[System.Serializable]
public class InventorySlot
{
    public ItemObject item;

    public int ID;

    public int amount;

    public InventorySlot(int id,ItemObject item, int amount)
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
