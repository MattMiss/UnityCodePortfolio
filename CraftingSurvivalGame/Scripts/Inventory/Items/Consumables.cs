using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Items/Consumables", fileName = "ConsumablesName.asset")]
public class Consumables : Item {
    public enum ConsumablesType {
        Bandage, Water
    }

    public ConsumablesType consumablesType;
}