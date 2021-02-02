using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon {

    public override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= shotTime)
            {
                anim.SetTrigger("Fire");
                cameraAnim.SetTrigger("shake");
                shotTime = Time.time + timeBetweenShots;
            }
        }
    }

    public void Fire()
    {
        Instantiate(projectile, shotPoint.position, transform.rotation);
    }
}
