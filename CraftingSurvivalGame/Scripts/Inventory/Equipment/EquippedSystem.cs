
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedSystem : MonoBehaviour
{
    public sEquipmentSlot headSlot, torsoSlot, legSlot, feetSlot;
    public sEquipmentSlot[] quickSlots;
    public QuickSlots quickSlotsUI;
    public GameObject itemInRightHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    public bool SetEquipmentSlot(eEquipmentSlotType slotType, ItemInstance item, out ItemInstance returnItem, int quickSlotIndex = -1){
        bool slotWasFull = false;
        ItemInstance foundItem = null;

        switch (slotType){
            case eEquipmentSlotType.headSlot:
                slotWasFull = headSlot.slotOccupied;
                if (slotWasFull){   
                    foundItem = headSlot.item;  
                }
                AddItemToSlot(headSlot, item);
                break;
            case eEquipmentSlotType.torsoSlot:
                slotWasFull = torsoSlot.slotOccupied;
                if (slotWasFull){   foundItem = torsoSlot.item;  }
                break;
            case eEquipmentSlotType.legSlot:
                slotWasFull = legSlot.slotOccupied;
                if (slotWasFull){   foundItem = legSlot.item;  }
                break;
            case eEquipmentSlotType.feetSlot:   
                slotWasFull = feetSlot.slotOccupied;
                if (slotWasFull){   foundItem = feetSlot.item;  }
                break;
            case eEquipmentSlotType.quickSlot:
                slotWasFull = quickSlots[quickSlotIndex].slotOccupied;
                if (slotWasFull){   
                    foundItem = quickSlots[quickSlotIndex].item;  
                }
                AddItemToQuickSlot(quickSlots[quickSlotIndex], item, quickSlotIndex);
                break;
        }

        returnItem = foundItem;

        return slotWasFull;
    }

    private void AddItemToSlot(sEquipmentSlot slot, ItemInstance item){
        slot.item = item;
        slot.slotOccupied = true;
    }

    private void AddItemToQuickSlot(sEquipmentSlot slot, ItemInstance item, int index){
        slot.item = item;
        slot.slotOccupied = true;
        quickSlots[index].item = item;
        quickSlots[index].slotOccupied = true;
        quickSlotsUI.SetQuickSlotImg(quickSlotsUI.quickSlots[index], item.item.icon);
        print("Item added to quickslot " + index + " and occupied is " + slot.slotOccupied);
    }

}
