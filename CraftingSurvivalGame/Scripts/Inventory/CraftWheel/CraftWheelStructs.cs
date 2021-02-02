using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct sButtonsInfo {
    public string name;
    public Sprite icon;
    public GameObject associatedLayer;
    public GameObject associatedButton;
    public eCraftIconType craftIconType; 
    public Item itemIfCraftType;

}

[System.Serializable]
public struct sCraftSingleLayer {
    public string layerName;
    public sButtonsInfo[] buttonsInfo;
}

[System.Serializable]
public struct sCraftTextItemUI {
    public TMPro.TMP_Text textName;
    public TMPro.TMP_Text textSeparator;
    public TMPro.TMP_Text textAmt;

}
