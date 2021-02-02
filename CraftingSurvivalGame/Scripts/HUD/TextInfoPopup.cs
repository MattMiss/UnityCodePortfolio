using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInfoPopup : MonoBehaviour
{
    public TMP_Text itemText;
    public Animation animation;
    public float animWaitTime;
    private float timeLeft;
    private bool textShowing = false;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = animWaitTime;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (textShowing){
            print(animWaitTime);
            if (timeLeft > 0){
                timeLeft -= Time.deltaTime;
            }else{
                animation.Play("InfoTextFadeOut");
                textShowing = false;           
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemName"></param>
    public void PlayItemCreatedInfoText(string itemName){
        timeLeft = animWaitTime;
        itemText.SetText(itemName + " Created");
        animation.Play("InfoTextSlideIn");
        textShowing = true;
        
    }
}
