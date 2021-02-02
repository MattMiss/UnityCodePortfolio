using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public Image itemIcon;
    public TMP_Text amountText;
    public TMP_Text weightText;
    public TMP_Text valueText;
    public GameObject highlightBG;
    public GameObject selectBG;
    public bool isSelected = false;
    [SerializeField]
    private InventoryUI inventoryUIRef;
    public ItemInstance itemInstanceRef;



    public void SetupItem(string itemName, Sprite icon, int itemAmt, float weight, float value, InventoryUI inventoryUIRef, ItemInstance itemInstance){
        nameText.SetText(itemName);
        itemIcon.sprite = icon;
        amountText.SetText(itemAmt.ToString());
        weightText.SetText(weight.ToString());
        valueText.SetText(value.ToString());
        this.inventoryUIRef = inventoryUIRef;
        itemInstanceRef = itemInstance;
    }

    public void MouseEntered(){
        HightlightItem(true);
    }

    public void MouseExited(){
        HightlightItem(false); 
    }

    public void Clicked(){
        isSelected = true;
        PickupItemUI lastItem = inventoryUIRef.GetPickupItemUI();
        if (lastItem != null && lastItem != this){
            lastItem.SelectItem(false);
            lastItem.isSelected = false;
            
        }
        SelectItem(true);
        inventoryUIRef.SetSelectedPickupInInv(this);
        inventoryUIRef.SetClickItemInfo(itemInstanceRef);
    }

    private void HightlightItem(bool highlight){
        if (highlight){
            highlightBG.SetActive(true);
        }else{
            highlightBG.SetActive(false);
        }
    }

    private void SelectItem(bool select){
        if (select){
            selectBG.SetActive(true);
        }else{
            selectBG.SetActive(false);
        }
    }

}
