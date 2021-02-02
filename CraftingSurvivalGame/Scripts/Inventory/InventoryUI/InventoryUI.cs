using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private List<PickupItem> inventory;
    public EquippedSystem equippedSystem;
    public GameObject playerRef, inventoryItemInfoPanel, footerBtnsRef, curContentPanel, chooseQuickslotPanel;
    public GameObject[] contentPanels, contentButtons;
    public Animation animationComponent;
    public string ItemInfoFadeInName;
    public Color32 normalColor, highlightColor;
    public GameObject currentBtnSelected, currentBtnHighlighted, equipText, useText;
    public GameObject selectLeftBtn, allBtn, toolsAndWepBtn, clothAndArmorBtn, consumablesBtn, craftMaterialsBtn, resourcesBtn, selectRightBtn;
    public QuickSlots quickSlotsRef;
    private InventorySystem inventorySystem;
    //public InventoryUI inventoryUIRef;
    private PickupItemUI curSelctedPickupInInv;
    private InventoryItemInfoUI inventoryItemInfoUI;
    private int selectedContentIndex = 0;
    private GameObject[] inventoryButtons;
    private bool canEquipSelectedItem = false, canUseSelectedItem = false;
    

    ///
    void OnEnable()
    {   
        inventorySystem = playerRef.GetComponent<InventorySystem>();
        refreshInventory();
        SetupButtons();
        curContentPanel = contentPanels[0];
        currentBtnSelected = allBtn;   
        selectedContentIndex = 0;
        ShowAllContent();
    }

    void OnDisable(){
        SetButtonColor(currentBtnSelected, false);
        curContentPanel.SetActive(false);
        equipText.SetActive(false);
        useText.SetActive(false);
    }

    void Start()
    {
        inventoryItemInfoUI = inventoryItemInfoPanel.GetComponent<InventoryItemInfoUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && curSelctedPickupInInv != null){
            inventorySystem.RemoveFromInventory(curSelctedPickupInInv.itemInstanceRef, false);  // ConsumeItem = false here since we are dropping the item
            refreshInventory();  
        }else if (Input.GetKeyDown(KeyCode.Z)){
            // Chnage inventory selection type Left
            print("Lefttt");
            SelectLeftContent();
        }else if (Input.GetKeyDown(KeyCode.X)){
            // Chnage inventory selection type Right
            print("Right");
            SelectRightContent();
        }else if (Input.GetKeyDown(KeyCode.F) && canEquipSelectedItem){
             ActivateChooseQuickSlot(!quickSlotsRef.settingQuickSlot);
        }else if (Input.GetKeyDown(KeyCode.G) && canUseSelectedItem){
            
        }
    }

    public PickupItemUI GetPickupItemUI(){
        return curSelctedPickupInInv;
    }

    // Once an item is clicked in the menu, decide whether it is a tool, weapon, clothing, or armor to Equip, 
    // or if its consumable to Use..
    public void SetSelectedPickupInInv(PickupItemUI pickup){
        curSelctedPickupInInv = pickup;
        ePickupType pickupType = pickup.itemInstanceRef.item.pickupType;
        if(pickupType == ePickupType.ToolsAndWeapons || pickupType == ePickupType.ClothingAndArmor){
            equipText.SetActive(true);
            canEquipSelectedItem = true;
            useText.SetActive(false);
            canUseSelectedItem = false;
        }else if (pickupType == ePickupType.Consumables){
            equipText.SetActive(true);
            canEquipSelectedItem = true;
            useText.SetActive(true);
            canUseSelectedItem = true;
        }else{
            equipText.SetActive(false);
            canEquipSelectedItem = false;
            useText.SetActive(false);
            canUseSelectedItem = false;
        }
    }

    public void refreshInventory()
    {
        ClearAll();
        
        for (int i = 0; i < Inventory.Instance.inventory.Length; i++)
        {
            ItemInstance item;
            bool getItem = Inventory.Instance.GetItem(i, out item);

            if (getItem) {
                contentPanels[0].GetComponent<ScrollViewItem>().AddContent(item, this);

                print(item.item.pickupType);
                switch(item.item.pickupType){
                    case ePickupType.ToolsAndWeapons:
                        contentPanels[1].GetComponent<ScrollViewItem>().AddContent(item, this);
                        break;
                    case ePickupType.ClothingAndArmor:
                        contentPanels[2].GetComponent<ScrollViewItem>().AddContent(item, this);
                        break;
                    case ePickupType.Consumables:
                        contentPanels[3].GetComponent<ScrollViewItem>().AddContent(item, this);
                        break;
                    case ePickupType.CraftingMaterials:
                        contentPanels[4].GetComponent<ScrollViewItem>().AddContent(item, this);
                        break;
                    case ePickupType.RawResources:
                        contentPanels[5].GetComponent<ScrollViewItem>().AddContent(item, this);
                        break;
                    case ePickupType.Coins:
                        //Add to money or dont even use
                        break;
                }
            }
        }      
    }

    public void SetupButtons(){
        inventoryButtons = new GameObject[6];
        inventoryButtons[0] = allBtn;
        inventoryButtons[1] = toolsAndWepBtn;
        inventoryButtons[2] = clothAndArmorBtn;
        inventoryButtons[3] = consumablesBtn;
        inventoryButtons[4] = craftMaterialsBtn;
        inventoryButtons[5] = resourcesBtn;
    }

    // Button OnClick functions
    public void ShowAllContent(){
        SetCurrentPage(contentPanels[0], allBtn, 0);    
    }

    public void ShowWepAndToolsContent(){
        SetCurrentPage(contentPanels[1], toolsAndWepBtn, 1); 
    }

    public void ShowClothAndArmorContent(){
        SetCurrentPage(contentPanels[2], clothAndArmorBtn, 2); 
    }

    public void ShowConsumablesContent(){
        SetCurrentPage(contentPanels[3], consumablesBtn, 3); 
    }

    public void ShowCraftMaterialsContent(){
        SetCurrentPage(contentPanels[4], craftMaterialsBtn, 4);   
    }

    public void ShowResourcesContent(){
        SetCurrentPage(contentPanels[5], resourcesBtn, 5);   
    }

    // Set as the main current page shown and set the buttons/colors as needed
    public void SetCurrentPage(GameObject layerToSet, GameObject btnToSet, int newPageIndex){
        // Set Page
        if (curContentPanel){
            curContentPanel.SetActive(false);
        }
        if (currentBtnSelected){
            SetButtonColor(currentBtnSelected, false);
        }
        layerToSet.SetActive(true);
        curContentPanel = layerToSet;   

         //Set Button
        selectedContentIndex = newPageIndex;
        currentBtnSelected = btnToSet;
        
        SetButtonColor(btnToSet, true);

        if (inventoryItemInfoUI && inventoryItemInfoUI.gameObject.activeSelf){
           inventoryItemInfoUI.gameObject.SetActive(false); 
        }

    }

    public void SetButtonColor(GameObject buttonLayer, bool highlight){
        if (highlight){
            buttonLayer.GetComponentInChildren<TMP_Text>().color = highlightColor;
        }else{
            currentBtnSelected.GetComponentInChildren<TMP_Text>().color = normalColor;
        }
    }

    public void SetClickItemInfo(ItemInstance itemInstance){

        if(!inventoryItemInfoUI.gameObject.activeSelf){
            animationComponent.Play(ItemInfoFadeInName);
        }
        inventoryItemInfoUI.SetItemInfo(itemInstance);
    }

    // if slot is occupied, do something with the item returned
    public void SetItemInQuickSlot(int quickSlotIndex){
        ItemInstance exchangedItem;
        equippedSystem.SetEquipmentSlot(eEquipmentSlotType.quickSlot, curSelctedPickupInInv.itemInstanceRef, out exchangedItem, quickSlotIndex);
    }

    public void ActivateChooseQuickSlot(bool activate){
        chooseQuickslotPanel.SetActive(activate);
        quickSlotsRef.settingQuickSlot = activate; 
    }

    private void ClearAll(){
        foreach(GameObject panel in contentPanels){
            try{
                foreach (Transform child in panel.GetComponent<ScrollViewItem>().contentPanel.transform) {
                    GameObject.Destroy(child.gameObject);
                }
            }catch{
                return;
            }
        } 
    }

    private void SelectLeftContent(){
        if (selectedContentIndex == 0){
            selectedContentIndex = contentPanels.Length - 1;
        }else{
            selectedContentIndex--;
        }
        SelectFromIndex(selectedContentIndex);
        //print(selectedContentIndex);
    }
    private void SelectRightContent(){
        if (selectedContentIndex == contentPanels.Length - 1){
            selectedContentIndex = 0;
        }else{
            selectedContentIndex++;
        }
        SelectFromIndex(selectedContentIndex);
        //print(selectedContentIndex);
    }

    private void SelectFromIndex(int index){
        print(index);
        switch(index){
            case 0:
                ShowAllContent();   
                break;
            case 1:
                ShowWepAndToolsContent();
                break;
            case 2:
                ShowClothAndArmorContent();
                break;
            case 3:
                ShowConsumablesContent();
                break;
            case 4:
                ShowCraftMaterialsContent();
                break;
            case 5:
                ShowResourcesContent();
                break;
        }
    }
}
