using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftText : MonoBehaviour
{
    [SerializeField]
    private sCraftTextItemUI item1, item2, item3;
    [SerializeField]
    private TMP_Text craftTextName, notEnoughTextName;
    [SerializeField]
    private Color32 canCraftColor, notEnoughColor;


    void Start()
    {
        craftTextName.color = canCraftColor;
        notEnoughTextName.color = notEnoughColor;
    }

    public void SetItemName(int itemIndex, string name){
        switch(itemIndex){
            case 0:
                SetItem1Name(name);
                break;
            case 1:
                SetItem2Name(name);
                break;
            case 2:
                SetItem3Name(name);
                break;
        }
    }

    public void SetItemAmt(int itemIndex, string amt){
        switch(itemIndex){
            case 0:
                SetItem1Amt(amt);
                break;
            case 1:
                SetItem2Amt(amt);
                break;
            case 2:
                SetItem3Amt(amt);
                break;
        }
    }

    
    public void ClearItem(int itemIndex){
        switch(itemIndex){
            case 0:
                item1.textName.text = "";
                item1.textSeparator.text = "";
                item1.textAmt.text = "";
                break;
            case 1:
                item2.textName.text = "";
                item2.textSeparator.text = "";
                item2.textAmt.text = "";
                break;
            case 2:
                item3.textName.text = "";
                item3.textSeparator.text = "";
                item3.textAmt.text = "";
                break;
        }
    }

    public void SetItem1Name(string name){
        item1.textName.text = name;
    }

    public void SetItem1Amt(string amt){
        item1.textAmt.text = amt;
    }

    public void SetItem2Name(string name){
        item2.textName.text = name;
    }

    public void SetItem2Amt(string amt){
        item2.textAmt.text = amt;
    }

    public void SetItem3Name(string name){
        item3.textName.text = name;
    }

    public void SetItem3Amt(string amt){
        item3.textAmt.text = amt;
    }

    public void SetCanCraft(bool canCraft){
        print("[CraftText.SetCanCraft] " + canCraft);

        notEnoughTextName.gameObject.SetActive(!canCraft);
        craftTextName.gameObject.SetActive(canCraft);  
    }
}
