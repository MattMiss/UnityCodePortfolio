using UnityEngine;

[System.Serializable]
public struct sAudioClipArray{
    public string clipArrayName;
    public eAudioClipArrayType audioClipArrayType;
    public AudioClip[] audioClips;
}