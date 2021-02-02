using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider waterSlider;
    public Slider foodSlider;
    public TMP_Text healthAmtText;
    public TMP_Text staminaAmtText;
    public TMP_Text waterAmtText;
    public TMP_Text foodAmtText;
    public PlayerVitals playerVitals;

    /// <summary>
    /// 
    /// </summary>
    void Start(){
        SetHealth(playerVitals.GetCurPlayerHealth());
        SetStamina(playerVitals.GetCurPlayerStamina());
        SetWater(playerVitals.GetCurPlayerWater());
        SetFood(playerVitals.GetCurPlayerFood());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="healthVal"></param>
    public void SetHealth(float healthVal){
        healthSlider.value = healthVal;
        healthAmtText.SetText(healthVal.ToString("0.00"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="staminaVal"></param>
    public void SetStamina(float staminaVal){
        staminaSlider.value = staminaVal;
        staminaAmtText.SetText(staminaVal.ToString("0.00"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="waterVal"></param>
    public void SetWater(float waterVal){
        waterSlider.value = waterVal;
        waterAmtText.SetText(waterVal.ToString("0.00"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="foodVal"></param>
    public void SetFood(float foodVal){
        foodSlider.value = foodVal;
        foodAmtText.SetText(foodVal.ToString("0.00"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="healthVal"></param>
    /// <param name="staminaVal"></param>
    /// <param name="waterVal"></param>
    /// <param name="foodVal"></param>
    public void SetSliderValues(float healthVal, float staminaVal, float waterVal, float foodVal){
        healthSlider.value = healthVal;
        staminaSlider.value = staminaVal;
        waterSlider.value = waterVal;
        foodSlider.value = foodVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="healthText"></param>
    /// <param name="staminaText"></param>
    /// <param name="waterText"></param>
    /// <param name="foodText"></param>
    public void SetTextValues(string healthText, string staminaText, string waterText, string foodText){
        healthAmtText.SetText(healthText);
        staminaAmtText.SetText(staminaText);
        waterAmtText.SetText(waterText);
        foodAmtText.SetText(foodText);
    }
}
