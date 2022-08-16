using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //References
    public GameObject bullet;
    public int currentAmmo, pickupAmount;
    public bool canAutoFire;
    public float fireRate;
    [HideInInspector]
    public float fireCounter;
    public Transform muzzlePointHolder;
    public float zoomAmount;
    public string gunName;
    public Color muzzleFlashColor;
    public float recoilAmount;

    [SerializeField]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }

    public void AddAmmo()
    {
        currentAmmo += pickupAmount;

        UIController.instance.ammoText.text = "AMMO: " + currentAmmo;
    }

    public void PlayFireAnim()
    {
        if (anim != null)
        {
            anim.SetTrigger("fire");
        }
    }
}
