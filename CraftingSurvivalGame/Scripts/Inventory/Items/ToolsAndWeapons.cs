using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Items/ToolsAndWeapons", fileName = "ToolsAndWeaponsName.asset")]
public class ToolsAndWeapons : Item {
    public enum ToolsAndWeaponsType {
        StoneHatchet, StonePickaxe, Torch, BoneKnife, StoneHammer, WoodenSpear, WoodenBow
    }

    public ToolsAndWeaponsType toolsAndWeaponsType;
}