using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string gunName;
    private bool collected;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected)
        {
            // Add ammo
            WeaponManager.instance.AddGun(gunName);
            AudioManager.instance.PlaySFX(4);
            collected = true;
            Destroy(gameObject);
        }
    }
}
