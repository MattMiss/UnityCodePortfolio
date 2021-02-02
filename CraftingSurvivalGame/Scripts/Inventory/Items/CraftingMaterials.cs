using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Items/CraftingMaterials", fileName = "CraftingMaterialsName.asset")]
public class CraftingMaterials : Item {
    public enum CraftingMaterialsType {
        Rope, Cloth
    }

    public CraftingMaterialsType craftingMaterialsType;
}