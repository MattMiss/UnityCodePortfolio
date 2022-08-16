using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    public string cpName;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))
        {
            if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_cp") == cpName)
            {
                PlayerControllerOld.instance.transform.position = transform.position;
                Physics.SyncTransforms();
                Debug.Log("Player Starting at Checkpoint: " + cpName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Remove checkpoint when "L" is pressed for debug purposes
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", "");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", cpName);
            Debug.Log("Player Reached Checkpoint: " + cpName);

            AudioManager.instance.PlaySFX(1);
        }
    }
}
