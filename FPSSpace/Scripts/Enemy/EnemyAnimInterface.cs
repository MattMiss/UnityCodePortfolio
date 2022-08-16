using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimInterface : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    public void WeaponFired(AnimationEvent animationEvent)
    {
        if (weapon)
        {
           // weapon.SendBullet();
        }
    }
}
