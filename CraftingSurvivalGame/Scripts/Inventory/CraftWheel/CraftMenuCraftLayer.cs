using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftMenuCraftLayer : MonoBehaviour{
    public Item itemToCreate;
    public bool canCraft;
    public Image separatorIcon;
    public Sprite backIcon;
    public float iconDistributionRadius, scaleOfSelectedIcon;
    public TextInfoPopup textInfoPopup;
    private Quaternion iconLocalRotOffset = Quaternion.Euler(0, 0, -45);
    private sCraftMaterialInfo materialsNeeded;
    [SerializeField]
    private CraftText craftText;
    private CraftMenuInnerLayer craftMenuInnerLayer;
    private GameObject craftIconLayer, selectedIcon, backButton;
    private int item1FoundIndex, item2FoundIndex, item3FoundIndex, amountOfIcons, selectedIconIndex;
    private float[] iconRotations = new float[2];
    private float selectionAngle;
    private Vector3 menuCenterPos = new Vector3(0,0,0);
    private CraftMenuMainLayer craftMenuMainLayer;

    /// <summary>
    /// 
    /// </summary>
    void OnEnable(){
        if (itemToCreate){
            materialsNeeded = itemToCreate.craftMaterialInfo;
            amountOfIcons = GetAmtOfMaterials(materialsNeeded) + 1;
            SetMaterialsNeededInfo();
        }

        // infoText.SetActive(false);    
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDisable(){
        canCraft = false;
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        craftText = GetComponentInChildren<CraftText>();
        craftMenuMainLayer = GetComponentInParent<CraftMenuMainLayer>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        // Always set the selection angle based on angle from center (0,0) while open
        selectionAngle = craftMenuMainLayer.GetAngleFromCenter();
        int btnIndexToSelect = CraftWheelUtils.GetHighlightedButton(amountOfIcons, selectionAngle);
        SetSelectedIconIndex(btnIndexToSelect);


        if (Input.GetKeyDown(KeyCode.E) && canCraft){
            print("Crafted " + itemToCreate.itemName);
            try {
                Inventory.Instance.InsertItem(new ItemInstance(itemToCreate));
                print("ITEMS Index: " + item1FoundIndex + " - " + item2FoundIndex + " - " + item3FoundIndex);
                List<int> items = ReverseOrder(item1FoundIndex, item2FoundIndex, item3FoundIndex);
                items.ForEach(item => {
                    try{
                        Inventory.Instance.RemoveItem(item);
                    }catch{
                        print("couldnt remove item");
                    }
                });
                print("ITEMTOCREATE: " + itemToCreate.itemName);
                try{
                    textInfoPopup.PlayItemCreatedInfoText(itemToCreate.itemName);
                }catch{
                    print("popup didn't work!");
                }
            }catch{
                print("[CraftMenuCraftLayer] - Something Went wrong");
            }
            SetMaterialsNeededInfo();
        }
        if (Input.GetKeyDown(KeyCode.L)){
            textInfoPopup.PlayItemCreatedInfoText("Something");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    /// <param name="indexC"></param>
    /// <returns></returns>//
    public List<int> ReverseOrder(int indexA, int indexB, int indexC){
        List<int> reversed = new List<int>();
        reversed.Add(indexA);
        reversed.Add(indexB);
        reversed.Add(indexC);

        reversed.Sort();
        reversed.Reverse();

        return reversed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentLayer"></param>
    public void SetParentMenu(CraftMenuInnerLayer parentLayer){
        craftMenuInnerLayer = parentLayer;
        craftMenuInnerLayer.DeactivateAfterSetup();
    }


    // MakeIconLayer gets called right after CraftMenuInnerLayer sets the itemToCreate here
    // and then the craft layer is set to active right after Icons are created
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonsInfo"></param>
    /// <returns></returns>
    public GameObject MakeIconLayer(sButtonsInfo buttonsInfo){
        craftIconLayer = new GameObject("CraftIconLayer");
        craftIconLayer.transform.SetParent(gameObject.transform, false);
        SetupLayerIcons(buttonsInfo, craftIconLayer);

        return craftIconLayer;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonsInfo"></param>
    /// <param name="newLayer"></param>
    private void SetupLayerIcons(sButtonsInfo buttonsInfo, GameObject newLayer){
        int btnAmt = GetAmtOfMaterials(buttonsInfo.itemIfCraftType.craftMaterialInfo);
        iconRotations = CraftWheelUtils.SetIconOffsets(btnAmt + 1);
        CreateIcons(btnAmt, buttonsInfo, newLayer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="btnAmt"></param>
    /// <param name="buttonsInfo"></param>
    /// <param name="layer"></param>
    private void CreateIcons(int btnAmt, sButtonsInfo buttonsInfo, GameObject layer)
    {        

        // Add one to account for Back button
        int btnLength = btnAmt + 1;

        GameObject[] newIconButtons = new GameObject[btnLength];

        for (int i = 0; i < btnLength; i++){
            // Reverse the indices
            int btnIndex = btnLength - 1 - i;

            Vector3 iconPosition;
            Quaternion iconRotation;
            CraftWheelUtils.GetCraftBtnPosition(btnLength, btnIndex, iconRotations[0], menuCenterPos, iconDistributionRadius, out iconPosition, out iconRotation);

            Vector3 btnPosition;
            Quaternion btnRotation;
            CraftWheelUtils.GetCraftBtnPosition(btnLength, btnIndex, iconRotations[1], menuCenterPos, iconDistributionRadius, out btnPosition, out btnRotation);

            Image separator = Instantiate(separatorIcon, iconPosition, iconRotation *= iconLocalRotOffset);
            separator.name = "MenuSeparatorIcon";
            separator.transform.SetParent(layer.transform, false);

            GameObject tmpIcon = new GameObject("tmpIcon", typeof(Image));

            // GameObject curIcon; 
            // MenuCreateButton curMBC;   

            Item tmpMaterial;

            switch(i){
                case 0:
                    tmpIcon.GetComponent<Image>().sprite = backIcon;
                    tmpIcon.name = "BackIcon";
                    tmpIcon.transform.position = btnPosition;
                    tmpIcon.transform.SetParent(layer.transform, false);
                    backButton = tmpIcon;
                    break;
                case 1:
                    tmpMaterial = buttonsInfo.itemIfCraftType.craftMaterialInfo.firstMaterial;
                    tmpIcon.GetComponent<Image>().sprite = tmpMaterial.icon;
                    tmpIcon.name = tmpMaterial.itemName;
                    tmpIcon.transform.position = btnPosition;
                    tmpIcon.transform.SetParent(layer.transform, false);
                    break;
                case 2:
                    tmpMaterial = buttonsInfo.itemIfCraftType.craftMaterialInfo.secondMaterial;
                    tmpIcon.GetComponent<Image>().sprite = tmpMaterial.icon;
                    tmpIcon.name = tmpMaterial.itemName;
                    tmpIcon.transform.position = btnPosition;
                    tmpIcon.transform.SetParent(layer.transform, false);
                    break;
                case 3:
                    tmpMaterial = buttonsInfo.itemIfCraftType.craftMaterialInfo.thirdMaterial;
                    tmpIcon.GetComponent<Image>().sprite = tmpMaterial.icon;
                    tmpIcon.name = tmpMaterial.itemName;
                    tmpIcon.transform.position = btnPosition;
                    tmpIcon.transform.SetParent(layer.transform, false);
                    break;
            }

            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="craftMaterialInfo"></param>
    /// <returns></returns>
    private int GetAmtOfMaterials(sCraftMaterialInfo craftMaterialInfo){
        int amt = 0;

        if(craftMaterialInfo.firstMaterial != null){
            amt++;
        }
        if(craftMaterialInfo.secondMaterial != null){
            amt++;
        }
        if(craftMaterialInfo.thirdMaterial != null){
            amt++;
        }
        return amt;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    private void SetSelectedIconIndex(int index){
        if(index != selectedIconIndex){
            if (selectedIcon) { selectedIcon.transform.localScale = new Vector3(1f, 1f, 1f); }
            selectedIconIndex = index;

            if (selectedIconIndex == 0){
                selectedIcon = backButton;
                selectedIcon.transform.localScale = new Vector3(scaleOfSelectedIcon, scaleOfSelectedIcon, scaleOfSelectedIcon);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetMaterialsNeededInfo(){
        bool item1Found = false;
        bool item2Found = false;
        bool item3Found = false;

        int item1Index;
        int item2Index;
        int item3Index;


        SetupItemCraftText(0, materialsNeeded.firstMaterial, materialsNeeded.firstAmtNeeded, out item1Index, out item1Found);
        SetupItemCraftText(1, materialsNeeded.secondMaterial, materialsNeeded.secondAmtNeeded, out item2Index, out item2Found);
        SetupItemCraftText(2, materialsNeeded.thirdMaterial, materialsNeeded.thirdAmtNeeded, out item3Index, out item3Found);


        print("[CraftMenuCraftLayer.SetMaterialsNeededInfo] " + item1Found + " - " + item2Found + " - " + item3Found);
        print("[CraftMenuCraftLayer.SetMaterialsNeededInfo] " + item1FoundIndex + " - " + item2FoundIndex + " - " + item3FoundIndex);

        if (item1Found && item2Found && item3Found) {
            item1FoundIndex = item1Index;
            item2FoundIndex = item2Index;
            item3FoundIndex = item3Index;
            craftText.SetCanCraft(true);
            canCraft = true;
        }else{
            craftText.SetCanCraft(false);
            canCraft = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="materialInfo"></param>
    /// <param name="materialAmtNeeded"></param>
    /// <param name="itemFoundIndex"></param>
    /// <param name="wasItemFound"></param>
    private void SetupItemCraftText(int itemIndex, Item materialInfo, int materialAmtNeeded, out int itemFoundIndex, out bool wasItemFound){
        bool foundItem = false;
        int index = -1;
        if (materialInfo != null){
            craftText.SetItemName(itemIndex, materialInfo.itemName);
            ItemInstance tmpItem1;
            if (Inventory.Instance.SearchForItem(new ItemInstance(materialInfo), out tmpItem1, out index)) {
                craftText.SetItemAmt(itemIndex, materialAmtNeeded + " [1]");
                //materialAmt1.color = foundItemColor;
                foundItem = true;
            }else{
                craftText.SetItemAmt(itemIndex, materialAmtNeeded + " [0]");
                //materialAmt1.color = notFoundItemColor;
            }
        }else{
            // Clear craftItem1Text
            print("Index " + itemIndex + " not found");
            craftText.ClearItem(itemIndex);
        }
        itemFoundIndex = index;
        wasItemFound = foundItem;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="craftOn"></param>
    public void SetCraftTextOnOff(bool craftOn){
        craftText.SetCanCraft(craftOn);
    }

    /// <summary>
    /// 
    /// </summary>
    public void IconClicked(){
        if(selectedIconIndex == 0){
            craftMenuInnerLayer.CloseCraftMenu();
        }
    }


}