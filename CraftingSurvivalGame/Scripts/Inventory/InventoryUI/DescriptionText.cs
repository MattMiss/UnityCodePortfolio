using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionText : MonoBehaviour
{
    private TMP_Text iconDescription;

    public TMP_Text GetIconDescription(){
        return iconDescription;
    }

    public void SetIconDescription(string desc){
        iconDescription.text = desc;
        //print(desc);
    }

    void Start()
    {
        iconDescription = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(iconDescription);
    }
}
