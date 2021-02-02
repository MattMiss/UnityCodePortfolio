using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CraftMenuMainLayer : MonoBehaviour{
    public PlayerMenu playerMenu;
    private FirstPersonLook firstPersonLook;
    private bool isCraftWheelShowing = false, setupDone = false, innerSetupDone = false;
    private float angleFromCenter = 0;
    private GameObject iconSelectBar;
    private CraftMenuInnerLayer craftMenuInnerLayer;
    


    public float GetAngleFromCenter()
    {
        return angleFromCenter;
    }
    public bool GetMenuShowing()
    {
        return isCraftWheelShowing;
    }

    // Grab a reference to the FirstPersonLook script in order to freeze look movement while craft menu is showing
    void Start()
    {
        firstPersonLook = FindObjectsOfType<FirstPersonLook>()[0];
        craftMenuInnerLayer = GetComponentInChildren<CraftMenuInnerLayer>();
        //craftMenuInnerLayer.gameObject.SetActive(false);
    }

    void Update()
    {
        // After all children components are setup, setupDone will become true
        // Once setupDone is true, hide the inner Menu
        if (!innerSetupDone && setupDone){
            if (craftMenuInnerLayer != null){
                craftMenuInnerLayer.gameObject.SetActive(false);
                innerSetupDone = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {   
            print(isCraftWheelShowing);

            if(!playerMenu.gameObject.activeSelf){
                ShowHideQuickCreateMenu(isCraftWheelShowing);  
            }
        }

        // Only calculate the cursor angle while the Craft Menu is open
        if (isCraftWheelShowing) 
        {  
            angleFromCenter = CalculateAngleFromCenter();
            // Check for Left Mouse click while Craft Menu is open (icon selection)
            if (Input.GetMouseButtonDown(0))
            {
                // Check whether the craft menu or inner menu is open during the click
                if(craftMenuInnerLayer.GetIsCraftMenuOpen()){
                    craftMenuInnerLayer.GetCraftLayer().IconClicked();
                }else{
                    // If Left Mouse Button is click, call the switchlayer function
                    //currentLayer.GetComponent<QuickCreateLayer>().SwitchLayer();
                    craftMenuInnerLayer.IconClicked();
                }
                
            }
        }   
    }


    public void ShowHideQuickCreateMenu(bool show)
    {
        craftMenuInnerLayer.gameObject.SetActive(!show);

        isCraftWheelShowing = !show;
        firstPersonLook.canLook = show;

        // Hide cursor if menu is hidden
        Cursor.visible = !show;

        if (!show){
            Cursor.lockState = CursorLockMode.None; 
        }else{
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    // Takes the MousePosition X and Y values and returns the angle from the center point (0, 0)
    // This is used to calculate which button/icon should be highlighted in the craft wheel

    // Return: returns a float Angle in degrees betwen -180 and 180. -180 to 0 is left, 0 to 180 is right.
    public float CalculateAngleFromCenter()
    {
        // screenPoint is the Vector3 from the center of the screen (0, 0);
        var screenPoint = new Vector3(Input.mousePosition.x - (Screen.width / 2) , Input.mousePosition.y - (Screen.height / 2), Input.mousePosition.z);
        
        return Mathf.Atan2(screenPoint.x, screenPoint.y) * Mathf.Rad2Deg * -1;
    }


    public void DeActivateInner(){
        setupDone = true;
    }

}