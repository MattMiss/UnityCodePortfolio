using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuCreateButton : MonoBehaviour {
    public string name;
    public Image icon;
    public GameObject associatedLayer;

    public void SetInfo(string name, Sprite sprite){
        this.name = name;
        icon = this.gameObject.GetComponent<Image>();
        icon.sprite = sprite;
    }

    public void SetInfo(string name, Sprite sprite, GameObject associatedLayer){
        this.name = name;
        icon = this.gameObject.GetComponent<Image>();
        icon.sprite = sprite;
        this.associatedLayer = associatedLayer;
    }

    
}