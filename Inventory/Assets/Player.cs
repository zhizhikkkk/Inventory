using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            inventory.Load();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
       
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            inventory.AddItem(new Item(groundItem.item), 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
       inventory.container.Items.Clear();
    }
}
