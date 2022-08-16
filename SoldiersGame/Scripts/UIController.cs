using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private TMP_Text livesText;
    
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text timeLastedText;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLivesText(int number)
    {
        livesText.text = number.ToString();
    }

    public void StartGame()
    {
        GameManager.instance.StartGame();
        startMenu.SetActive(false);
    }

    public void ShowPauseMenu(bool show)
    {
        pauseMenu.SetActive(show);
    }

    public void ShowGameOver()
    {
        gameOverMenu.SetActive(true);
        timeLastedText.text = timeText.text;
    }

    public void SetTimeText(string time)
    {
        timeText.text = time;
    }
}
