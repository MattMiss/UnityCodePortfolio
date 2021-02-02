using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSoundEvents : MonoBehaviour
{
    public AudioObject playerLegAudioObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstep(){
        int clipIndex = playerLegAudioObject.FindClipArrayIndex(eAudioClipArrayType.footsteps);
        print(clipIndex);
        playerLegAudioObject.RandomizeAndPlayClip(clipIndex);
    }

}
