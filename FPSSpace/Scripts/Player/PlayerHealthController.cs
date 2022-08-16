using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [SerializeField]
    private int maxhealth;
    private int currentHealth;

    [SerializeField]
    private float invincibleLength = 1f;
    private float invincibleCounter;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxhealth;

        UIController.instance.healthSlider.maxValue = maxhealth;
        UpdateUIHealth();
    }

    void Update()
    {
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if (invincibleCounter <= 0 && !GameManager.instance.levelEnding)
        {
            currentHealth -= damageAmount;

            UIController.instance.ShowDamage();
            AudioManager.instance.PlaySFX(7);

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);

                currentHealth = 0;
                GameManager.instance.PlayerDied();
                AudioManager.instance.StopBGM();
                AudioManager.instance.PlaySFX(6);
                AudioManager.instance.StopSFX(7);
            }

            invincibleCounter = invincibleLength;

            UpdateUIHealth();
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxhealth)
        {
            currentHealth = maxhealth;
        }
        UpdateUIHealth();
    }

    private void UpdateUIHealth()
    {
        UIController.instance.healthSlider.value = currentHealth;
    }
}
