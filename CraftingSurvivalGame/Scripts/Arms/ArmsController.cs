using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsController : MonoBehaviour
{
    public Animator animatorLeftArm, animatorRightArm;
    public bool action1Occuring = false, endOfSwing = false, grabbingToolOccuring = false;
    public GameObject weaponRSocket;
    public QuickSlots quickSlots;
    public AudioObject armsAudioObject, arms2AudioObject;
    public EquippedSystem equippedSystem;
    private bool isRunning = false, puttingItemAway = false, canPlaySwingSound = true;
    private Vector2 velocity;
    private float moveVelocityToUse;



    /// <summary>
    /// 
    /// </summary>
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.G) && !isRunning){
            GrabTrigger();
        }

        if (velocity.x > velocity.y){
            moveVelocityToUse = velocity.x;
        }else{
            moveVelocityToUse = velocity.y;
        }

        animatorRightArm.SetFloat("forwardSpeed", moveVelocityToUse);
        animatorLeftArm.SetFloat("forwardSpeed", moveVelocityToUse);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="velocityY"></param>
    /// <param name="velocityX"></param>
    public void SetVelocity(float velocityY, float velocityX){
        this.velocity.y = velocityY;
        this.velocity.x = velocityX;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isRunning"></param>
    public void SetIsRunning(bool isRunning){
        this.isRunning = isRunning;
    }

    /// <summary>
    /// 
    /// </summary>
    public void GrabTrigger(){
        animatorLeftArm.SetTrigger("grabItem");
    }

    /// <summary>
    /// 
    /// </summary>
    public void Action1Trigger(){
        if (!action1Occuring){
            animatorRightArm.SetTrigger("action1");
            action1Occuring = true;
        }
    }

    public void PlaySwingToolSound(){
        if (equippedSystem.itemInRightHand && canPlaySwingSound){
            int clipIndex = armsAudioObject.FindClipArrayIndex(eAudioClipArrayType.swingingTool);
            armsAudioObject.RandomizeAndPlayClip(clipIndex);
            canPlaySwingSound = false;
        }
        /// ELSE PUNCH SOUND
    }

    // gets trigger by animation at the last 10 or 15 percent of animation when pickupitem trigger is enabled
    public void SwingOpen(){
        endOfSwing = true;
    }

    public void SwingClosed(){
        endOfSwing = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Action1Finished(){
        action1Occuring = false;
        canPlaySwingSound = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grabbingItem"></param>
    public void GrabToolWeaponTrigger(bool grabbingItem){
        if (!grabbingToolOccuring){
            animatorRightArm.SetTrigger("grabToolWeapon");
            grabbingToolOccuring = true;
        }
        puttingItemAway = !grabbingItem;
    }

    /// <summary>
    /// 
    /// </summary>
    public void GrabToolFinished(){
        grabbingToolOccuring = false;
        if (puttingItemAway){
            Destroy(quickSlots.equippedSystem.itemInRightHand);
            SetAnimationLayer(0);
        }else{
            quickSlots.SpawnItemInHand();
            SetAnimationLayer(1);
        }
    }

    
    // THIS can change to a different ENUM depending on the weapon/item type of hold animation
    // 0 = Unarmed, 1 = upright tool (hatchet)
    // Layer index 1 is the tool Layer and a float of 0f becomes unarmed while 1f becomes tool layer
    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeInt"></param>
    public void SetAnimationLayer(int typeInt){
        switch(typeInt){
            case 0:
                animatorRightArm.SetLayerWeight(1, 0f);
                break;
            case 1:
                animatorRightArm.SetLayerWeight(1, 1f);
                break;
        }
    }


}
