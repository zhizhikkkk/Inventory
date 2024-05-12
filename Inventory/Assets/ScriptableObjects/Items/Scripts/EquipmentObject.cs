using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Item/Equipment")]
public class EquipmentObject : ItemObject
{
    public float atkBonus;
    public float defenseBonus;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
