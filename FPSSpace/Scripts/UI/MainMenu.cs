using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string firstLevel;

    [SerializeField]
    private GameObject continueButton;

    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            if (PlayerPrefs.GetString("CurrentLevel") == "")
            {
                continueButton.SetActive(false);
            }
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(firstLevel);
        PlayerPrefs.SetString("CurrentLevel", "");
        PlayerPrefs.SetString(firstLevel + "_cp", "");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));        
    }
}
