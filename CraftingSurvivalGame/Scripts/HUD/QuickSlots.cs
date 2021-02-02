using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlots : MonoBehaviour
{
    public GameObject[] quickSlots;
    public ArmsController armsController;
    public EquippedSystem equippedSystem;
    public ItemInHandUI itemInHandUIRef;
    public InventoryUI inventoryUIRef;
    public bool settingQuickSlot = false;
    private GameObject spawnObject;
    private int spawnObjectSlotIndex;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void QuickSlotActivated(int index){
        print("Quickslot " + index + " clicked");

        if (settingQuickSlot){
            inventoryUIRef.SetItemInQuickSlot(index);
            inventoryUIRef.ActivateChooseQuickSlot(false);
        }else{
            sEquipmentSlot slotChosen = equippedSystem.quickSlots[index];
            if (slotChosen.slotOccupied){
                //Check if occupied item is already equipped. If it is, put it away
                if (slotChosen.equippedInHand){
                    EquipInHand(false, index, itemInHandUIRef.bareHands);
                }else{
                    EquipInHand(true, index, slotChosen.item.item.icon, slotChosen.item.item.physicalRepresentation);
                }
            }
        }    
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quickSlot"></param>
    /// <param name="sprite"></param>
    public void SetQuickSlotImg(GameObject quickSlot, Sprite sprite){
        quickSlot.GetComponent<QuickSlot>().SetSlotIcon(sprite);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SpawnItemInHand(){
        GameObject itemInRightHand = Instantiate(spawnObject, armsController.weaponRSocket.transform, false);
        itemInRightHand.GetComponent<PickupItem>().NowInHand(armsController);
        equippedSystem.quickSlots[spawnObjectSlotIndex].equippedInHand = true;
        equippedSystem.itemInRightHand = itemInRightHand;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grabWeapon"></param>
    /// <param name="slotIndex"></param>
    /// <param name="iconToSet"></param>
    /// <param name="objectToSpawn"></param>
    private void EquipInHand(bool grabWeapon, int slotIndex, Sprite iconToSet, GameObject objectToSpawn = null){
        armsController.GrabToolWeaponTrigger(grabWeapon);
        quickSlots[slotIndex].GetComponent<QuickSlot>().HighlightSlot(grabWeapon);
        itemInHandUIRef.SetIconInHand(iconToSet);
        if (!grabWeapon){
            equippedSystem.quickSlots[slotIndex].equippedInHand = false;
        }else{
            spawnObject = objectToSpawn;
        }
    }
}
