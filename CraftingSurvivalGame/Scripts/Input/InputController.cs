using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public ArmsController armsController;
    public QuickSlots quickSlots;
    public PlayerMenuController playerMenuController;
    public PauseMenu pauseMenuRef;
    public GameObject crossHair;
    public InventoryUI inventoryUIRef;
    public Jump jump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        crossHair.SetActive(!IsAnyMenuShowing());

        if (Input.GetMouseButtonDown(0) && !IsAnyMenuShowing()){
            armsController.Action1Trigger();
        }else if (Input.GetKeyDown(KeyCode.C)){
            playerMenuController.KeyPressed(eMenuPageEnum.characterPage);
        }else if(Input.GetKeyDown(KeyCode.I)){
            playerMenuController.KeyPressed(eMenuPageEnum.inventoryPage);
        }else if(Input.GetKeyDown(KeyCode.K)){
            playerMenuController.KeyPressed(eMenuPageEnum.skillsPage);
        }else if(Input.GetKeyDown(KeyCode.J)){
            playerMenuController.KeyPressed(eMenuPageEnum.questsPage);
        }else if(Input.GetKeyDown(KeyCode.M)){
            playerMenuController.KeyPressed(eMenuPageEnum.mapPage);
        }else if (Input.GetKeyDown(KeyCode.Alpha1)){
            quickSlots.QuickSlotActivated(0);
        }else if (Input.GetKeyDown(KeyCode.Alpha2)){
            quickSlots.QuickSlotActivated(1);
        }else if (Input.GetKeyDown(KeyCode.Alpha3)){
            quickSlots.QuickSlotActivated(2);
        }else if (Input.GetKeyDown(KeyCode.Alpha4)){
            quickSlots.QuickSlotActivated(3);
        }else if (Input.GetKeyDown(KeyCode.Alpha5)){
            quickSlots.QuickSlotActivated(4);
        }else if (Input.GetKeyDown(KeyCode.Alpha6)){
            quickSlots.QuickSlotActivated(5);
        }else if (Input.GetKeyDown(KeyCode.Alpha7)){
            quickSlots.QuickSlotActivated(6);
        }else if (Input.GetKeyDown(KeyCode.Alpha8)){
            quickSlots.QuickSlotActivated(7);
        }else if (Input.GetKeyDown(KeyCode.Alpha9)){
            quickSlots.QuickSlotActivated(8);
        }else if (Input.GetKeyDown(KeyCode.Alpha0)){
            quickSlots.QuickSlotActivated(9);
        }else if (Input.GetKeyDown(KeyCode.Escape)){
            if (quickSlots.settingQuickSlot){
                inventoryUIRef.ActivateChooseQuickSlot(false);
            }else{
                pauseMenuRef.ShowHidePauseMenu(!pauseMenuRef.GetPauseMenuShowing());
            }
        }
    }

    void LateUpdate(){
        if (Input.GetButtonDown("Jump")){
            jump.TryJump();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool IsAnyMenuShowing(){    
        return playerMenuController.GetAnyMenuShowing() || pauseMenuRef.GetPauseMenuShowing();
    }
}
