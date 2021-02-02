using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewItem : MonoBehaviour
{
    public GameObject contentPanel;
    public GameObject pickupItemUI;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddContent(ItemInstance item, InventoryUI inventoryUIRef){

        GameObject newUIItem = Instantiate(pickupItemUI);
        PickupItemUI itemUI = newUIItem.GetComponent<PickupItemUI>();
        itemUI.SetupItem(item.item.itemName, item.item.icon, 1, item.item.weight, item.item.value, inventoryUIRef, item);
        newUIItem.transform.SetParent(contentPanel.transform, true);
    }
}
