using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject playerMenuHolder;
    public FirstPersonMovement firstPersonMovement;
    public FirstPersonLook firstPersonLook;
    private List<PickupItem> inventory = new List<PickupItem>();
    private InventoryUI inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        firstPersonMovement = GetComponent<FirstPersonMovement>();
        firstPersonLook = GetComponentInChildren<FirstPersonLook>();
        inventoryUI = GetComponent<InventoryUI>();
    }

    public List<PickupItem> GetInventory()
    {
        return inventory;
    }

    public void ShowHidePlayerMenu(bool show, int index = 0)
    {
        if (show){
            playerMenuHolder.GetComponent<PlayerMenu>().curPageIndex = index;
        }
        // Show / Hide Inventory Canvas
        playerMenuHolder.SetActive(show);

        // Lock / Unlock Movement and Look
        firstPersonMovement.canMove = !show;
        firstPersonLook.canLook = !show;

        // Lock / Unlock Cursor
        Cursor.visible = show;
        if (show){
            Cursor.lockState = CursorLockMode.None; 
        }else{
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void AddToInventory(GameObject pickup)
    {
        //inventory.Add(pickup);
        //print("Added new item " + pickup.name);
        Inventory.Instance.InsertItem(new ItemInstance(item: pickup.GetComponent<PhysicalItem>().scriptableObjectRepresentation));
        Destroy(pickup);
        //print("added " + itemToAdd.item.name);
    }

    // Removes the item from Inventory. If item is consumeItem, it will just be deleted.
    // If its not consumeItem, it will drop
    public void RemoveFromInventory(ItemInstance itemToRemove, bool consumeItem){
        ItemInstance searchFind;
        int searchFindIndex;
        Inventory.Instance.SearchForItem(itemToRemove, out searchFind, out searchFindIndex);

        if (searchFind != null){
            Inventory.Instance.RemoveItem(searchFindIndex);
            if (!consumeItem){
                DropInstanceOnGround(itemToRemove);
            } 
        }  
    }

    public void DropInstanceOnGround(ItemInstance itemToDrop){
        GameObject createdItem = Instantiate(itemToDrop.item.physicalRepresentation, gameObject.transform.position, Quaternion.identity);
    }



}
