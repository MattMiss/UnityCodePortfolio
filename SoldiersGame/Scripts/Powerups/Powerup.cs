using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    [SerializeField] protected PowerupIconChooser.PowerupType powerupType;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PickupPowerup();
        }
    }

    public virtual void PickupPowerup()
    {
        Destroy(gameObject);
    }
}
