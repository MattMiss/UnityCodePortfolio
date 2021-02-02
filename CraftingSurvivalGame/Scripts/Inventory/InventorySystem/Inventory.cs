using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Inventory.cs
[CreateAssetMenu(menuName = "Items/Inventory", fileName = "Inventory.asset")]
[System.Serializable]
public class Inventory : ScriptableObject {
    // Saving using unity dev example.
    // https://bitbucket.org/richardfine/scriptableobjectdemo/src/9a60686609a42fea4d00f5d20ffeb7ae9bc56eb9/Assets/ScriptableObject/GameSession/GameSettings.cs?at=default#GameSettings.cs-16,79,83,87,90
    private static Inventory _instance;
    public static Inventory Instance {
        get {
            if (!_instance) {
                Inventory[] tmp = Resources.FindObjectsOfTypeAll<Inventory>();
                if (tmp.Length > 0) {
                    _instance = tmp[0];
                    Debug.Log("Found inventory as: " + _instance);
                } else {
                    Debug.Log("Did not find inventory, loading from file or template.");
                    SaveManager.LoadOrInitializeInventory();
                }
            }

            return _instance;
        }
    }

    public static void InitializeFromDefault() {
        if (_instance) DestroyImmediate(_instance);
        _instance = Instantiate((Inventory) Resources.Load("InventoryTemplate"));
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public static void LoadFromJSON(string path) {
        if (_instance) DestroyImmediate(_instance);
        _instance = ScriptableObject.CreateInstance<Inventory>();
        JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public void SaveToJSON(string path) {
        Debug.LogFormat("Saving inventory to {0}", path);
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    /* Inventory START */
    public ItemInstance[] inventory;

    public bool SlotEmpty(int index) {
        if (inventory[index] == null || inventory[index].item == null)
            return true;

        return false;
    }

    // Get an item if it exists.
    public bool GetItem(int index, out ItemInstance item) {
        // inventory[index] doesn't return null, so check item instead.
        if (SlotEmpty(index)) {
            item = null;
            return false;
        }

        item = inventory[index];
        return true;
    }

    // Search and return an item if it exists.
    public bool SearchForItem(ItemInstance searchItem, out ItemInstance item, out int itemIndex) {

        bool found = false;
        item = null;
        itemIndex = -1;

        if (inventory.Length > 0){
            Debug.Log("Inventory Length : " + inventory.Length);
            for (int i = 0; i < inventory.Length; i++){
                try{
                    if (searchItem.item == inventory[i].item){
                        item = inventory[i];
                        found = true;
                        itemIndex = i;
                        break;
                    }
                }catch{

                }
                
            }
        }
        
        
        return found;
    }

    // Remove an item at an index if one exists at that index.
    public bool RemoveItem(int index) {
        if (SlotEmpty(index)) {
            // Nothing existed at the specified slot.
            return false;
        }

        inventory[index] = null;

        return true;
    }

    // Insert an item, return the index where it was inserted.  -1 if error.
    public int InsertItem(ItemInstance item) {
        for (int i = 0; i < inventory.Length; i++) {
            if (SlotEmpty(i)) {
                inventory[i] = item;
                return i;
            }
        }

        // Couldn't find a free slot.
        return -1;
    }

    // Simply save.
    private void Save() {
        SaveManager.SaveInventory();
    }
}