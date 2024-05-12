using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Inventory",menuName="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
   public List<InventorySlot> container = new List<InventorySlot>();

    public void AddItem(ItemObject item,int amount) {
        bool hasitem = false;
        for(int i=0;i<container.Count;i++)
        {
            if (container[i].item ==item)
            {
                container[i].AddAmount(amount);
                hasitem = true;
                break;
            }
        }

        if(!hasitem)
        {
            container.Add(new InventorySlot(item,amount));
        }
    }

}


[System.Serializable]
public class InventorySlot
{
    public ItemObject item;

   

    public int amount;

    public InventorySlot(ItemObject item,int amount)
    {

        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int  value)
    {
        amount += value;
    }
}
