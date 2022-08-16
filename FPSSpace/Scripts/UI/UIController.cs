using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("References")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;

    [Header("Damage Screen")]
    public Image damageEffect;
    public float damageAlpha = .25f, damageFadeSpeed = 2f;

    [Header("Pause Screen")]
    public GameObject pauseScreen;
    
    [Header("Fade Screen")]
    public Image blackScreen;
    [SerializeField]
    private float fadeSpeed = 1.5f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
    }

    void Update()
    {
        // Fade damage effect out
        if (damageEffect.color.a != 0)
        {
            damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, Mathf.MoveTowards(damageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));
        }

        if (!GameManager.instance.levelEnding)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        }
        else
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        }
    }

    public void ShowDamage()
    {
        damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, damageAlpha);
    }
}
