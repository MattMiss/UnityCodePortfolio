using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemInfoUI : MonoBehaviour
{
    public TMP_Text itemName;
    public Image itemIcon;
    public TMP_Text itemWeight;
    public TMP_Text itemValue;
    public TMP_Text itemDescription;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInfo(ItemInstance itemInstance){
        itemName.SetText(itemInstance.item.itemName);
        itemWeight.SetText(itemInstance.item.weight + " LB");
        itemValue.SetText(itemInstance.item.value + " P");
        itemDescription.SetText(itemInstance.item.itemDescription);

        itemIcon.sprite = itemInstance.item.icon;
    }

}
