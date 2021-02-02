using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Items/ClothingAndArmor", fileName = "ClothingAndArmorName.asset")]
public class ClothingAndArmor : Item {
    public enum ClothingAndArmorType {
        ClothBoots, ClothLeggings, ClothShirt, ClothHat
    }

    public ClothingAndArmorType clothingAndArmorType;
}