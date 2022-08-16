using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float rangeToTargetPlayer, timeBetweenShots = .5f, rotateSpeed = 5f;
    private float shotCounter;
    [SerializeField]
    private Transform turretWeapon, muzzlePointLeft, muzzlePointRight;

    void Start()
    {
        shotCounter = timeBetweenShots;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerControllerOld.instance.transform.position) < rangeToTargetPlayer)
        {
            turretWeapon.LookAt(PlayerControllerOld.instance.transform.position);

            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                Instantiate(bullet, muzzlePointLeft.position, muzzlePointLeft.rotation);
                Instantiate(bullet, muzzlePointRight.position, muzzlePointRight.rotation);
                shotCounter = timeBetweenShots;
            }
        }
        else
        {
            shotCounter = timeBetweenShots;
            turretWeapon.rotation = Quaternion.Lerp(turretWeapon.rotation, Quaternion.Euler(0f, turretWeapon.rotation.eulerAngles.y + 10f, 0f), rotateSpeed * Time.deltaTime);
        }

    }
}
