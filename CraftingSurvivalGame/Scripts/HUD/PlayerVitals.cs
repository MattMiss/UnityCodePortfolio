using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    [SerializeField]
    private float playerMaxHealth = 100f;
    [SerializeField]
    private float playerMaxStamina = 100f;
    [SerializeField]
    private float playerMaxWater = 100f;
    [SerializeField]
    private float playerMaxFood = 100f;
    private float curPlayerHealth, curPlayerStamina, curPlayerWater, curPlayerFood;
    private bool isRunning = false;
    public PlayerStatsUI playerStatsUI;
    public CharacterUI characterUI;
    public FirstPersonMovement firstPersonMovement;
    public float waterDropRate = .5f, foodDropRate = .4f;  // % dropped per real life minute
    public float foodWaterCheckRate = 2.0f, staminaRunDropAmt = .2f, staminaRefreshAmt = .2f;
    private float foodWaterTimeRemaining, staminaDropTimeRemaining, staminaRefreshTimeRemaining, staminaDropCheckRate = .1f, staminaRefreshCheckRate = .1f;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        ResetFoodWaterTimer();
        ResetStaminaTimer();
        curPlayerHealth = playerMaxHealth;
        curPlayerStamina = playerMaxStamina;
        curPlayerWater = playerMaxWater;
        curPlayerFood = playerMaxFood;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            DamagePlayer(10f);
        }else if (Input.GetKeyDown(KeyCode.O)){
            HealPlayer(5f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate() {      
        foodWaterTimeRemaining -= 1.0f / foodWaterCheckRate * Time.deltaTime;
        if (foodWaterTimeRemaining <= 0){
            LowerFoodAndWater();
            ResetFoodWaterTimer();
        }

        // Lower Stamina slowly while running
        if (isRunning){
            staminaDropTimeRemaining -= Time.deltaTime;
            if (staminaDropTimeRemaining <= 0){
                LowerStamina();
                ResetStaminaTimer();
            }
            // Stop running if stamina drops too low
            if (curPlayerStamina <= staminaRunDropAmt){
                firstPersonMovement.StopRunning();
            }
        }else{
            // Refresh and raise stamina slowly while not running
            if (curPlayerStamina < playerMaxStamina){
                staminaRefreshTimeRemaining -= Time.deltaTime;
                if (staminaRefreshTimeRemaining <= 0){
                    RefreshStamina();
                    ResetStaminaTimer();
                }
            }
        }


        //print(vitalTimeRemaining);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResetFoodWaterTimer(){
        foodWaterTimeRemaining = foodWaterCheckRate;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResetStaminaTimer(){
        staminaDropTimeRemaining = staminaDropCheckRate;
        staminaRefreshTimeRemaining = staminaRefreshCheckRate;
    }

    /// <summary>
    /// 
    /// </summary>
    private void LowerFoodAndWater(){
        curPlayerWater -= waterDropRate;
        curPlayerFood -= foodDropRate;

        playerStatsUI.SetWaterAmt(curPlayerWater / playerMaxWater);
        characterUI.SetWater(curPlayerWater);
        playerStatsUI.SetFoodAmt(curPlayerFood / playerMaxFood);
        characterUI.SetFood(curPlayerFood);
    }

    /// <summary>
    /// 
    /// </summary>
    private void LowerStamina(){
        curPlayerStamina -= staminaRunDropAmt;

        playerStatsUI.SetStaminaAmt(curPlayerStamina / playerMaxStamina);
        characterUI.SetStamina(curPlayerStamina);
    }

    /// <summary>
    /// 
    /// </summary>
    private void RefreshStamina(){
        curPlayerStamina += staminaRefreshAmt;

        // Cap it at max stamina
        if (curPlayerStamina > playerMaxStamina){
            curPlayerStamina = playerMaxStamina;
        }
        playerStatsUI.SetStaminaAmt(curPlayerStamina / playerMaxStamina);
        characterUI.SetStamina(curPlayerStamina);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetCurPlayerHealth(){
        return curPlayerHealth;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetCurPlayerStamina(){
        return curPlayerStamina;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetCurPlayerWater(){
        return curPlayerWater;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetCurPlayerFood(){
        return curPlayerFood;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="run"></param>
    public void SetRunning(bool run){
        isRunning = run;
        ResetStaminaTimer();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void LowerStaminaFromAction(float amount){
        curPlayerStamina -= amount;
        if (curPlayerStamina < 0){
            curPlayerStamina = 0;
        }
        playerStatsUI.SetStaminaAmt(curPlayerStamina / playerMaxStamina);
        characterUI.SetStamina(curPlayerStamina);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void DamagePlayer(float amount){
        curPlayerHealth -= amount;
        playerStatsUI.PlayerHitAnimationUI();
        if (curPlayerHealth < 0){
            //Player is DEAD

        }
        playerStatsUI.SetHealthAmt(curPlayerHealth / playerMaxHealth);
        characterUI.SetHealth(curPlayerHealth);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void HealPlayer(float amount){
        curPlayerHealth += amount;
        if (curPlayerHealth > playerMaxHealth){
            curPlayerHealth = playerMaxHealth;
        }
        playerStatsUI.SetHealthAmt(curPlayerHealth / playerMaxHealth);
        characterUI.SetHealth(curPlayerHealth);
    }
}
