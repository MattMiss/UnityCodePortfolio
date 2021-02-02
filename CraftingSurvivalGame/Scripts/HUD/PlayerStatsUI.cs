using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image staminaBar;
    [SerializeField]
    private Image waterBar;
    [SerializeField]
    private Image foodBar;
    public Animation animationComponent;
    public string hitAnimName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fillAmount"></param>
    public void SetHealthAmt(float fillAmount){
        healthBar.fillAmount = fillAmount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fillAmount"></param>
    public void SetStaminaAmt(float fillAmount){
        staminaBar.fillAmount = fillAmount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fillAmount"></param>
    public void SetWaterAmt(float fillAmount){
        waterBar.fillAmount = fillAmount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fillAmount"></param>
    public void SetFoodAmt(float fillAmount){
        foodBar.fillAmount = fillAmount;
    }   

    /// <summary>
    /// 
    /// </summary>
    public void PlayerHitAnimationUI(){
        animationComponent.Play(hitAnimName);
    }






    
}