using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private float waitAfterDying = 2f;

    private DefaultInput defaultInput;

    [HideInInspector]
    public bool levelEnding;

    public bool gamePaused = false;

    void Awake()
    {
        instance = this;

        defaultInput = new DefaultInput();

        defaultInput.Game.Escape.performed += e => PauseUnpause();
  
        defaultInput.Enable();
    }

    void OnDisable()
    {
        defaultInput.Game.Escape.performed -= e => PauseUnpause();
  
        defaultInput.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void PlayerDied()
    {
        StartCoroutine(PlayerDiedCo());
    }

    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseUnpause()
    {
        if (gamePaused)
        {
            UIController.instance.pauseScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gamePaused = false;

            Time.timeScale = 1f;

            //PlayerControllerOld.instance.footStepSlow.Play();
            //PlayerControllerOld.instance.footstepFast.Play();
        }
        else
        {
            UIController.instance.pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            gamePaused = true;

            Time.timeScale = 0f;

            //PlayerControllerOld.instance.footStepSlow.Stop();
            //PlayerControllerOld.instance.footstepFast.Stop();
        }
    }
}
