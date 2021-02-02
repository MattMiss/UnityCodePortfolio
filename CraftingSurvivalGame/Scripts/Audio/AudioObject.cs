using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public sAudioClipArray[] audioClipStruct;
    private AudioSource audioSource;
    private int lastIndexPlayed;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.clip = audioClips[0];
    }

    public void PlaySound(){
        audioSource.Play();
    }
    

    public void RandomizeAndPlayClip(int clipArrayIndex){
        // Dont play sound if index isn't found
        if (clipArrayIndex != -1){
            int rInt = Random.Range(0, audioClipStruct[clipArrayIndex].audioClips.Length - 1);
            while (rInt == lastIndexPlayed){
                rInt = Random.Range(0, audioClipStruct[clipArrayIndex].audioClips.Length - 1);
            }
            lastIndexPlayed = rInt;
            audioSource.clip = audioClipStruct[clipArrayIndex].audioClips[rInt];
            
            audioSource.Play();
        }
    }

    public int FindClipArrayIndex(eAudioClipArrayType clipArrayType){
        int foundIndex = -1;

        for (int i = 0; i < audioClipStruct.Length; i++){
            print(audioClipStruct[i].audioClipArrayType + " and " + clipArrayType);
            if (audioClipStruct[i].audioClipArrayType == clipArrayType){
                foundIndex = i;
                break;
            }
        }
        return foundIndex;
    }

}
