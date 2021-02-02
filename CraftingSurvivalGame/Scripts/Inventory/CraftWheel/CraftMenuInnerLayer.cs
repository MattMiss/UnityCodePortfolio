using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CraftMenuInnerLayer : MonoBehaviour{
    
    public Image separatorIcon;
    public float iconDistributionRadius, scaleOfSelectedIcon;
    public sCraftSingleLayer[] craftLayers;
    private CraftMenuCraftLayer craftMenuCraftLayer;
    private CraftMenuMainLayer craftMenuMainLayer;
    private Quaternion iconLocalRotOffset = Quaternion.Euler(0, 0, -45);
    private float selectionAngle;
    private float[] iconRotations = new float[2];
    private int amountOfIcons, selectedIconIndex;
    [SerializeField]
    private GameObject selectedIcon, currentLayerRef, returnMenuRef;
    private Vector3 menuCenterPos = new Vector3(0, 0, 0);
    private DescriptionText descriptionText;
    private sCraftSingleLayer firstMenuLayer, currentMenuLayer, returnLayerInfo;
    private bool isCraftMenuOpen = false;


    public bool GetIsCraftMenuOpen(){
        return isCraftMenuOpen;
    }

    public CraftMenuCraftLayer GetCraftLayer(){
        return craftMenuCraftLayer;
    }

    void OnEnable(){

        if (firstMenuLayer.buttonsInfo == null){
            print("e: first time");
            currentMenuLayer = craftLayers[0];
            amountOfIcons = craftLayers[0].buttonsInfo.Length;
        }     
    }

    void OnDisable(){
        Reset();
    }

    

    void Start(){
        // Find the layers needed in the children
        SetupChildComponents();
        // Go through each craftlayer info and create a layer for each item
        SetupMenuLayers();
    }

    void Update(){

        if (!isCraftMenuOpen){
            // Always set the selection angle based on angle from center (0,0) while open
            selectionAngle = craftMenuMainLayer.GetAngleFromCenter();
            int btnIndexToSelect = CraftWheelUtils.GetHighlightedButton(amountOfIcons, selectionAngle);
            SetSelectedIconIndex(btnIndexToSelect);
        }
    }

    public void DeactivateAfterSetup(){
        //gameObject.SetActive(false);
        print(craftMenuCraftLayer);
        craftMenuMainLayer.DeActivateInner();
    }

    private void SetupChildComponents(){
        craftMenuMainLayer = GetComponentInParent<CraftMenuMainLayer>();
        descriptionText = GetComponentInChildren<DescriptionText>();
        craftMenuCraftLayer = GetComponentInChildren<CraftMenuCraftLayer>();
        craftMenuCraftLayer.SetParentMenu(this);
    }

    private void SetupMenuLayers(){
        // Start off setting up the Main Craft menu which needs to be index 0 of buttonsInfo[]
        // After Main, all following buttonsInfo items will depend on the Name and AssociatedLayer
        // and the order of indices don't matter, although maintaining order helps to visualize menus.
        firstMenuLayer = craftLayers[0];
        
        GameObject firstLayer = CreateSingleMenuLayer("MainMenu", this.gameObject);
        SetupLayerIcons(firstMenuLayer, firstLayer);
        currentLayerRef = firstLayer;
        
        // Cascade down craftLayer tree until everything is Instantiated and connected via AssociatedLayer
        /*
            LAYER 1
        */
        for(int i = 0; i < firstMenuLayer.buttonsInfo.Length; i++){

            // If i == 0 , it will be the Exit button
            if (i == 0){

            }else{
                string searchName = firstMenuLayer.buttonsInfo[i].name;
                Debug.Log("b: " + searchName);
                sCraftSingleLayer foundLayer;
                // This will be false for MainMenu EXIT button
                if (SearchForLayerByName(searchName, out foundLayer)){              
                    GameObject newLayer = CreateSingleMenuLayer(searchName+"Menu", this.gameObject);
                    SetupLayerIcons(foundLayer, newLayer);

                    firstMenuLayer.buttonsInfo[i].associatedLayer = newLayer;
                    newLayer.SetActive(false);

                    /*
                        LAYER 2
                    */
                    for(int j = 0; j < foundLayer.buttonsInfo.Length; j++){

                        // If j == 0 , it will be the Back button
                        if(j == 0){
                            foundLayer.buttonsInfo[j].associatedLayer = firstLayer;
                        }else{
                            searchName = foundLayer.buttonsInfo[j].name;
                            Debug.Log("c: " + searchName);
                            sCraftSingleLayer foundLayerInner;
                            // This will be false for Back buttons at index 0
                            if (SearchForLayerByName(foundLayer.buttonsInfo[j].name, out foundLayerInner)){
                                GameObject newLayerInner = CreateSingleMenuLayer(searchName+"Menu", this.gameObject);
                                SetupLayerIcons(foundLayerInner, newLayerInner);
                                foundLayer.buttonsInfo[j].associatedLayer = newLayerInner;
                                newLayerInner.SetActive(false);

                                /*
                                    LAYER 2
                                */
                                for(int k = 0; k < foundLayerInner.buttonsInfo.Length; k++){

                                    // If j == 0 , it will be the Back button
                                    if(k == 0){
                                        foundLayerInner.buttonsInfo[k].associatedLayer = newLayer;
                                    }else{
                                        searchName = foundLayerInner.buttonsInfo[k].name;
                                        Debug.Log("c: " + searchName);
                                        sCraftSingleLayer foundLayerInnerMost;
                                        // This will be false for Back buttons at index 0
                                        if (SearchForLayerByName(foundLayerInner.buttonsInfo[k].name, out foundLayerInnerMost)){
                                            GameObject newLayerInnerMost = CreateSingleMenuLayer(searchName+"Menu", this.gameObject);
                                            SetupLayerIcons(foundLayerInnerMost, newLayerInnerMost);
                                            foundLayerInner.buttonsInfo[k].associatedLayer = newLayerInnerMost;
                                            newLayerInnerMost.SetActive(false);
                                        }
                                    }
                                }
                            }
                        }  
                    }
                }
            }

            
        }
        
    }



    private void SetupLayerIcons(sCraftSingleLayer foundLayerData, GameObject newLayer){
        iconRotations = CraftWheelUtils.SetIconOffsets(foundLayerData.buttonsInfo.Length);
        CreateIcons(foundLayerData, newLayer);
    }


     private GameObject CreateSingleMenuLayer(string layerName, GameObject parentObject){

        GameObject createdLayer = new GameObject(layerName);
        createdLayer.transform.SetParent(parentObject.transform, false);

        return createdLayer;
    }

    private bool SearchForLayerByName(string searchName, out sCraftSingleLayer craftSingleLayer){
        sCraftSingleLayer singleLayer = new sCraftSingleLayer();
        bool found = false;

        for (int i = 0; i < craftLayers.Length; i++){
            
            if (searchName == craftLayers[i].layerName){
                singleLayer = craftLayers[i];
                found = true;               
            }
        }

        craftSingleLayer = singleLayer;
        return found;
    }


    private void CreateIcons(sCraftSingleLayer layerData, GameObject layer)
    {        

        // Add one to account for Back button
        int btnLength = layerData.buttonsInfo.Length;

        GameObject[] newIconButtons = new GameObject[btnLength];

        //float angle = 360f / (float)btnLength;
        for (int i = 0; i < btnLength; i++)
        {
            // Reverse the indices
            int btnIndex = btnLength - 1 - i;
    
            Vector3 iconPosition;
            Quaternion iconRotation;
            CraftWheelUtils.GetCraftBtnPosition(btnLength, i, iconRotations[0], menuCenterPos, iconDistributionRadius, out iconPosition, out iconRotation);

            Vector3 btnPosition;
            Quaternion btnRotation;
            CraftWheelUtils.GetCraftBtnPosition(btnLength, i, iconRotations[1], menuCenterPos, iconDistributionRadius, out btnPosition, out btnRotation);

            Image separator = Instantiate(separatorIcon, iconPosition, iconRotation *= iconLocalRotOffset);
            separator.name = "MenuSeparatorIcon";
            separator.transform.SetParent(layer.transform, false);

            GameObject curIcon = new GameObject(layerData.buttonsInfo[btnIndex].name + "Icon", typeof(Image));
            MenuCreateButton curMBC = curIcon.AddComponent<MenuCreateButton>();
            curMBC.SetInfo(layerData.buttonsInfo[btnIndex].name, layerData.buttonsInfo[btnIndex].icon);
            layerData.buttonsInfo[btnIndex].associatedButton = curIcon;
             
            curIcon.transform.position = btnPosition;
            curIcon.transform.SetParent(layer.transform, false);
        } 
    }


    private void SetSelectedIconIndex(int index){
        if(index != selectedIconIndex){
            if (selectedIcon) { selectedIcon.transform.localScale = new Vector3(1f, 1f, 1f); }
            
            selectedIconIndex = index;
            selectedIcon = currentMenuLayer.buttonsInfo[index].associatedButton;
            selectedIcon.transform.localScale = new Vector3(scaleOfSelectedIcon, scaleOfSelectedIcon, scaleOfSelectedIcon);
              
            descriptionText.SetIconDescription(selectedIcon.GetComponent<MenuCreateButton>().name);
        }
    }

    private void SetSelectedLayer(sCraftSingleLayer layer, bool isBackLayer){

        if (currentLayerRef){
            currentLayerRef.SetActive(false);   
            print("currentRefFound");
        }else{
            print("No currentRefFound");
        }

        sCraftSingleLayer newLayerInfo;
        string tmpSearchName;
        
        if (isBackLayer){
            tmpSearchName = layer.buttonsInfo[selectedIconIndex].associatedLayer.name;
            tmpSearchName = tmpSearchName.Substring(0, tmpSearchName.Length-4);
        }else{
            tmpSearchName = layer.buttonsInfo[selectedIconIndex].name;
        }

        if(SearchForLayerByName(tmpSearchName, out newLayerInfo)){
            //print(newLayerInfo.buttonsInfo.Length);
            print("found layer: " + newLayerInfo.layerName);
            amountOfIcons = newLayerInfo.buttonsInfo.Length;
        }
        currentLayerRef = layer.buttonsInfo[selectedIconIndex].associatedLayer;
        currentMenuLayer = newLayerInfo;
        currentLayerRef.SetActive(true);
    }

    public void SetCraftSelectedLayer(sCraftSingleLayer layer){
        if (currentLayerRef){
            returnMenuRef = currentLayerRef;
            returnLayerInfo = layer;
            currentLayerRef.SetActive(false);   
        }
        

        // Chnage from description Text in the middle circle to Craft Text
        descriptionText.gameObject.SetActive(false);
        craftMenuCraftLayer.itemToCreate = layer.buttonsInfo[selectedIconIndex].itemIfCraftType;
        currentLayerRef = craftMenuCraftLayer.MakeIconLayer(layer.buttonsInfo[selectedIconIndex]);
        isCraftMenuOpen = true;
        craftMenuCraftLayer.gameObject.SetActive(true);
    }


    public void CloseCraftMenu(){
        if (currentLayerRef){
            Destroy(currentLayerRef);  
        }
        currentLayerRef = returnMenuRef;
        currentMenuLayer = returnLayerInfo;
        currentLayerRef.SetActive(true);
        isCraftMenuOpen = false;

        // Chnage back to description Text in the middle circle from Craft Text
        descriptionText.gameObject.SetActive(true);
        craftMenuCraftLayer.itemToCreate = null;
        craftMenuCraftLayer.SetCraftTextOnOff(false);
        craftMenuCraftLayer.gameObject.SetActive(false);
    }

    public void IconClicked(){
        
        // Close the entire menu if index is 0 and it's the Exit button on the MainMenu and not just a Back button
        if (selectedIconIndex == 0 && currentMenuLayer.buttonsInfo[0].craftIconType == eCraftIconType.exitLayerType){
            craftMenuMainLayer.ShowHideQuickCreateMenu(true);   
            Debug.Log("Clicked Exit button on Menu");
        }else if (selectedIconIndex == 0 && currentMenuLayer.buttonsInfo[0].craftIconType == eCraftIconType.backLayerType){
            SetSelectedLayer(currentMenuLayer, true);
            Debug.Log("Clicked Back button on Menu");
        }else{
            // Check if the button is a craft type (lowest level button) or just an icontype
            if (currentMenuLayer.buttonsInfo[selectedIconIndex].craftIconType == eCraftIconType.craftType){
                SetCraftSelectedLayer(currentMenuLayer);
            }else{
                SetSelectedLayer(currentMenuLayer, false);
            }
        } 
    }

    public void Reset(){
        if (currentLayerRef){
            currentLayerRef.SetActive(false);
        }
        craftMenuCraftLayer.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(true);
        currentMenuLayer = craftLayers[0];
        currentLayerRef = craftLayers[1].buttonsInfo[0].associatedLayer;
        currentLayerRef.SetActive(true);
        amountOfIcons = craftLayers[0].buttonsInfo.Length;
    }
}