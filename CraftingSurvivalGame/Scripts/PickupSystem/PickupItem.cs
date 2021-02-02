using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;



public enum ePickupType
{
    ToolsAndWeapons,
    ClothingAndArmor,
    Consumables,
    CraftingMaterials,
    RawResources,
    Coins
}

public class PickupItem : MonoBehaviour
{
    [System.Serializable]
    public class PickupInfo
    {
        public ePickupType pickupType;
        public string name;
        public float weight;
        public float value;
        public int amount;
        public Sprite icon;
        public GameObject Object;

    }

    public GameObject floatingText;
    public PickupInfo pickupInfo;
    public Collider pickupCollider, swingHitCollider;
    private GameObject lookAtSpot, playerRef;
    private PickupSystem pickupSystem;
    private bool showText = false;
    private Outlinable outlinable;
    private ArmsController armsControllerRef;


    // Start is called before the first frame update
    void Start()
    {
        outlinable = GetComponent<Outlinable>();
        playerRef = GameObject.Find("Player");
        pickupSystem = playerRef.GetComponent<PickupSystem>();
        lookAtSpot = GameObject.Find("LookAtSpot");
    }

    // Update is called once per frame
    void Update()
    {
        if (showText)
        {
            floatingText.transform.LookAt(lookAtSpot.transform);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pickupSystem.highlightedItem = this;
            floatingText.SetActive(true);
            outlinable.enabled = true;
            showText = true;
        }else if (armsControllerRef && armsControllerRef.endOfSwing && other.tag == "TreeChoppable"){
            other.GetComponent<TreeChoppable>().SingleChop(25, swingHitCollider.transform.position);
            int clipIndex = armsControllerRef.arms2AudioObject.FindClipArrayIndex(eAudioClipArrayType.treeChop);
            armsControllerRef.arms2AudioObject.RandomizeAndPlayClip(clipIndex);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pickupSystem.highlightedItem = null;
            floatingText.SetActive(false);
            outlinable.enabled = false;
            showText = false;
        }
    } 


    public void NowInHand(ArmsController armsController){
        pickupCollider.enabled = false;
        swingHitCollider.enabled = true;
        armsControllerRef = armsController;
    }
}
