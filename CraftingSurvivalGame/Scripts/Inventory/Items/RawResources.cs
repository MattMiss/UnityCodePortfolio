using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Items/RawResources", fileName = "RawResourcesName.asset")]
public class RawResources : Item {
    public enum RawResourceType {
        Stone, Stick, Log, Leather, Fur, Straw, Bone
    }

    public RawResourceType rawResourceType;
}