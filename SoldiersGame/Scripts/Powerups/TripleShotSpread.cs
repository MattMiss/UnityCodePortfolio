using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotSpread : Powerup
{
    [SerializeField] float powerupLength;

    public override void PickupPowerup()
    {
        PlayerController.instance.ChangeFireType(powerupType, powerupLength);
    
        base.PickupPowerup();
    }
}
