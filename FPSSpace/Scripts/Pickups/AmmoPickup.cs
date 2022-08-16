using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private bool collected;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected)
        {
            // Add ammo
            PlayerControllerOld.instance.activeGun.AddAmmo();
            AudioManager.instance.PlaySFX(3);
            collected = true;
            Destroy(gameObject);
        }
    }
}
