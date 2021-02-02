using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinuzi : Weapon {


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
                Instantiate(projectile, shotPoint.position, transform.rotation);
                cameraAnim.SetTrigger("shake");
                shotTime = Time.time + timeBetweenShots; 
            }
            anim.SetBool("isFiring", true);
        }
        else
        {
            anim.SetBool("isFiring", false);
        }
    }
}
