using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    public Image slotIcon;
    [SerializeField]
    private GameObject equippedBorder;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSlotIcon(Sprite sprite){
        slotIcon.sprite = sprite;
        MakeSlotOpaque(slotIcon);
    }

    /// <summary>
    /// 
    /// </summary>
    public void EmptySlot(){
        slotIcon.sprite = null;
        MakeSlotTransparent(slotIcon);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="highlight"></param>
    public void HighlightSlot(bool highlight){
        equippedBorder.SetActive(highlight);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="icon"></param>
    private void MakeSlotOpaque(Image icon){
        Color opaque = icon.color;
        opaque.a = 1f;
        icon.color = opaque;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="icon"></param>
    private void MakeSlotTransparent(Image icon){
        Color transparent = icon.color;
        transparent.a = 0f;
        icon.color = transparent;
    }



}
