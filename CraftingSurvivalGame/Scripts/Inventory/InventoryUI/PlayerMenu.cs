using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMenu : MonoBehaviour
{
    public int curPageIndex = 0;
    public GameObject characterPage, inventoryPage, skillsPage, questsPage, mapPage, inventoryHolder;
    public Animation pageAnimation;
    private GameObject currentPage;
    public GameObject selectLeftBtn, characterBtn, inventoryBtn, skillsBtn, questsBtn, mapBtn, selectRightBtn;
    public GameObject currentBtnSelected, currentBtnHighlighted;
    private GameObject[] mainLayerButtons;
    public Color32 normalColor, highlightColor;
    public eMenuPageEnum menuPageEnum;


    void Start(){
        SetupButtons();
        currentPage = inventoryPage;
        currentBtnSelected = inventoryBtn;   
        curPageIndex = 1;
        ShowInventoryPage();
    }

    void OnEnable(){

        switch(curPageIndex){
            case 0:
                ShowCharPage();
                break;
            case 1:
                ShowInventoryPage();
                break;
            case 2:
                ShowSkillsPage();
                break;
            case 3:
                ShowQuestsPage();
                break;
            case 4:
                ShowMapPage();
                break;
        }
        
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Q)){
            SelectToLeft();
        }else if (Input.GetKeyDown(KeyCode.E)){
            SelectToRight();
        }
    }

    public void SetupButtons(){
        mainLayerButtons = new GameObject[5];
        mainLayerButtons[0] = characterBtn;
        mainLayerButtons[1] = inventoryBtn;
        mainLayerButtons[2] = skillsBtn;
        mainLayerButtons[3] = questsBtn;
        mainLayerButtons[4] = mapBtn;
    }

    


    // Button OnClick functions
    public void ShowCharPage(){
        SetCurrentPage(characterPage, characterBtn, 0, "CharacterFadeIn");    
    }

    public void ShowInventoryPage(){
        SetCurrentPage(inventoryPage, inventoryBtn, 1, "InventoryFadeIn"); 
    }

    public void ShowSkillsPage(){
        SetCurrentPage(skillsPage, skillsBtn, 2, "SkillsFadeIn"); 
    }

    public void ShowQuestsPage(){
        SetCurrentPage(questsPage, questsBtn, 3, "QuestsFadeIn"); 
    }

    public void ShowMapPage(){
        SetCurrentPage(mapPage, mapBtn, 4, "MapFadeIn");   
    }


    // Set as the main current page shown and set the buttons/colors as needed
    public void SetCurrentPage(GameObject layerToSet, GameObject btnToSet, int newPageIndex, string animationName){
        // Set Page
        if (currentPage){
            currentPage.SetActive(false);
        }
        if (currentBtnSelected){
            SetButtonColor(currentBtnSelected, false);
        }
        layerToSet.SetActive(true);
        currentPage = layerToSet;   

         //Set Button
        curPageIndex = newPageIndex;
        currentBtnSelected = btnToSet;
        
        SetButtonColor(btnToSet, true);


        pageAnimation.Play(animationName); 
    }



    public void SelectButton(GameObject buttonLayer){
        if (buttonLayer != currentBtnSelected){
            buttonLayer.GetComponentInChildren<TMP_Text>().color = highlightColor;
        }
    }

    public void UnselectButton(GameObject buttonLayer){
        if (buttonLayer != currentBtnSelected){
            buttonLayer.GetComponentInChildren<TMP_Text>().color = normalColor;
        }
    }



    public void SelectToRight(){
        if (curPageIndex == mainLayerButtons.Length - 1){
            curPageIndex = 0;
        }else{
            curPageIndex++;
        }
        SelectFromIndex(curPageIndex);
    }

    public void SelectToLeft(){
        if (curPageIndex == 0){
            curPageIndex = mainLayerButtons.Length - 1;
        }else{
            curPageIndex--;
        }
        SelectFromIndex(curPageIndex);
    }

    private void SelectFromIndex(int index){
        switch(index){
            case 0:
                ShowCharPage();   
                break;
            case 1:
                ShowInventoryPage();
                break;
            case 2:
                ShowSkillsPage();
                break;
            case 3:
                ShowQuestsPage();
                break;
            case 4:
                ShowMapPage();
                break;
        }
    }

    
    public void SetButtonColor(GameObject buttonLayer, bool highlight){
        if (highlight){
            buttonLayer.GetComponentInChildren<TMP_Text>().color = highlightColor;
        }else{
            currentBtnSelected.GetComponentInChildren<TMP_Text>().color = normalColor;
        }
    }

}
