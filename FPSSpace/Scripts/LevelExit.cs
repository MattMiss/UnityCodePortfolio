using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private string nextLevel;

    [SerializeField]
    private float waitToEndLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.levelEnding = true;
            AudioManager.instance.PlayLevelComplete();
            StartCoroutine(EndLevelCo());
        }
    }

    private IEnumerator EndLevelCo()
    {
        // Clear checkpoints on the next level before loading
        PlayerPrefs.SetString(nextLevel + "_cp", "");
        PlayerPrefs.SetString("CurrentLevel", nextLevel);

        yield return new WaitForSeconds(waitToEndLevel);

        SceneManager.LoadScene(nextLevel);
    }
}
