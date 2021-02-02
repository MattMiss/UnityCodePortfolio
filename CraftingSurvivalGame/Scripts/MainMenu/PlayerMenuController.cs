using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{

    public PlayerMenu playerMenuRef;
    public InventorySystem inventorySystemRef;
    private bool isCharShowing, isInvShowing, isSkillsShowing, isQuestsShowing, isMapShowing;
    private bool anyMenuShowing = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    public bool GetAnyMenuShowing(){
        return anyMenuShowing;
    }

    public void KeyPressed(eMenuPageEnum menuPageBtnPressed){
        switch(menuPageBtnPressed){
            case eMenuPageEnum.characterPage:
                if (isCharShowing){
                    inventorySystemRef.ShowHidePlayerMenu(false);
                    anyMenuShowing = false;
                }else if(anyMenuShowing){
                    playerMenuRef.ShowCharPage();
                }else{
                    inventorySystemRef.ShowHidePlayerMenu(true, 0);
                    anyMenuShowing = true;
                }
                isCharShowing = !isCharShowing;
                break;
            case eMenuPageEnum.inventoryPage:
                if (isInvShowing){
                    inventorySystemRef.ShowHidePlayerMenu(false);
                    anyMenuShowing = false;
                }else if(anyMenuShowing){
                    playerMenuRef.ShowInventoryPage();
                }else{
                    inventorySystemRef.ShowHidePlayerMenu(true, 1);
                    anyMenuShowing = true;
                }
                isInvShowing = !isInvShowing;
                break;
            case eMenuPageEnum.skillsPage:
                if (isSkillsShowing){
                    inventorySystemRef.ShowHidePlayerMenu(false);
                    anyMenuShowing = false;
                }else if(anyMenuShowing){
                    playerMenuRef.ShowSkillsPage();
                }else{
                    inventorySystemRef.ShowHidePlayerMenu(true, 2);
                    anyMenuShowing = true;
                }
                isSkillsShowing = !isSkillsShowing;
                break;
            case eMenuPageEnum.questsPage:
                if (isQuestsShowing){
                    inventorySystemRef.ShowHidePlayerMenu(false);
                    anyMenuShowing = false;
                }else if(anyMenuShowing){
                    playerMenuRef.ShowQuestsPage();
                }else{
                    inventorySystemRef.ShowHidePlayerMenu(true, 3);
                    anyMenuShowing = true;
                }
                isQuestsShowing = !isQuestsShowing;
                break;
            case eMenuPageEnum.mapPage:
                if (isMapShowing){
                    inventorySystemRef.ShowHidePlayerMenu(false);
                    anyMenuShowing = false;
                }else if(anyMenuShowing){
                    playerMenuRef.ShowMapPage();
                }else{
                    inventorySystemRef.ShowHidePlayerMenu(true, 4);
                    anyMenuShowing = true;
                }
                isMapShowing = !isMapShowing;
                break;
        }
    }

}
