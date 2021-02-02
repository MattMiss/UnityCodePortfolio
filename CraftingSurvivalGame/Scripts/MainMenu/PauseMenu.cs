using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsScreen, pauseScreen;
    public string mainMenuScene;
    private bool pauseMenuShowing;
    public GameObject loadingScreen, loadingIcon;
    public TMP_Text loadingText;
    private FirstPersonMovement firstPersonMovement;
    private FirstPersonLook firstPersonLook;


    // Start is called before the first frame update
    void Start()
    {
        firstPersonMovement = GetComponent<FirstPersonMovement>();
        firstPersonLook = GetComponentInChildren<FirstPersonLook>();
    }

    public void ShowHidePauseMenu(bool show)
    {
        // Show / Hide Inventory Canvas
        pauseScreen.SetActive(show);
        pauseMenuShowing = show;

        // Lock / Unlock Movement and Look
        firstPersonMovement.canMove = !show;
        firstPersonLook.canLook = !show;

        // Lock / Unlock Cursor
        Cursor.visible = show;
        if (show){
            Cursor.lockState = CursorLockMode.None; 
        }else{
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool GetPauseMenuShowing(){
        return pauseMenuShowing;
    }

    public void OpenOptions(){
        optionsScreen.SetActive(true);
    }

    public void CloseOptions(){
        optionsScreen.SetActive(false);
    }

    public void QuitToMain(){
        StartCoroutine(LoadMain());
    }

    public IEnumerator LoadMain(){
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuScene);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone){
            if(asyncLoad.progress >= .9f){
                loadingText.text = "Press any key to continue...";
                loadingIcon.SetActive(false);

                if (Input.anyKeyDown){
                    asyncLoad.allowSceneActivation = true;
                    Time.timeScale = 1f;
                }
            }

            yield return null;
        }
    }
}
