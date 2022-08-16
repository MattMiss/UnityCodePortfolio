using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioSource bgm, levelComplete;

    [SerializeField]
    private AudioSource[] soundFX;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlayLevelComplete()
    {
        StopBGM();
        levelComplete.Play();
    }

    public void PlaySFX(int sfxIndex)
    {
        soundFX[sfxIndex].Stop();
        soundFX[sfxIndex].Play();
    }

    public void StopSFX(int sfxIndex)
    {
        soundFX[sfxIndex].Stop();
    }
}
