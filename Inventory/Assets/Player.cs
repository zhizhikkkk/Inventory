using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            Debug.Log("Inventory saved.");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            inventory.Load();
            Debug.Log("Inventory loaded.");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            Debug.Log($"Picking up item {groundItem.item.name}");
            inventory.AddItem(new Item(groundItem.item), 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.container.Items.Clear();
        Debug.Log("Inventory cleared on application quit.");
    }
} 