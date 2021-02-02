using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public PickupItem highlightedItem;
    public InventorySystem inventorySystem;

    private CraftMenuMainLayer craftMenuMainLayer;

    void Start()
    {
        inventorySystem = GetComponent<InventorySystem>();
        craftMenuMainLayer = FindObjectOfType<CraftMenuMainLayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (craftMenuMainLayer != null && !craftMenuMainLayer.GetMenuShowing()))
        {
            if (highlightedItem != null)
            {
                //PickupItem itemToAdd = Instantiate(highlightedItem);
                inventorySystem.AddToInventory(highlightedItem.gameObject);
                // Add item to inventory
                //Destroy(highlightedItem.gameObject);
            }
        }
    }
}
