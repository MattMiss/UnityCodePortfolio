using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot : MonoBehaviour {
    public int index = 0;
    public ItemInstance itemInstance = null;    // Inventory backend representation.
    public GameObject prefabInstance = null;    // Inventory frontend representation.

    // TODO: it would be better if we used SetActive() etc rather than Instantiate/Destroy.
    // Use this method to set a slot's item.
    // The slot will automatically instantiate the gameobject associated with the item.
    public void SetItem(ItemInstance instance) {
        this.itemInstance = instance;
        this.prefabInstance = Instantiate(instance.item.physicalRepresentation, transform);
    }

    // Remove the item from the slot, and destroy the associated gameobject.
    public void RemoveItem() {
        this.itemInstance = null;
        Destroy(this.prefabInstance);
        this.prefabInstance = null;
    }
}