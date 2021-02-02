using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// CraftMaterialInfo structure determines what 3 items are needed to craft 
// the new item and their amounts needed as well

[System.Serializable]
public struct sCraftMaterialInfo 
{
    public Item firstMaterial;
    public int firstAmtNeeded;
    public Item secondMaterial;
    public int secondAmtNeeded;
    public Item thirdMaterial;
    public int thirdAmtNeeded;
}

// Base structure for all items used/crafted in the game. 
[System.Serializable]
public abstract class Item : ScriptableObject {
    
    public string itemName;
    public float weight;
    public float value;
    public string itemDescription;
    public Sprite icon;
    public GameObject physicalRepresentation;
    public sCraftMaterialInfo craftMaterialInfo;
    public ePickupType pickupType;

}


// A class that holds a real instance of a ScriptableObject item.
// Allows us to have copies with mutable data.
[System.Serializable]
public class ItemInstance {
    // Reference to scriptable object "template".
    public Item item;
    // Object-specific data.
    //public Quality.QualityGrade quality;

    public ItemInstance(Item item) {
        this.item = item;
        //this.quality = quality;
    }
}