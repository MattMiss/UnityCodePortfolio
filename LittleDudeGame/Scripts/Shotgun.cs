using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

    public Transform shotPoint2;
    public Transform shotPoint3;


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
                Vector2 direction2 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
                float angle3 = angle2 - 15;
                float angle4 = angle2 + 15;

                if (isFlipped == false)
                {
                    angle3 = angle2 + 15;
                    angle4 = angle2 - 15;
                }

                Quaternion rotation2 = Quaternion.AngleAxis(angle3, Vector3.forward);
                Quaternion rotation3 = Quaternion.AngleAxis(angle4, Vector3.forward);

                Instantiate(projectile, shotPoint.position, transform.rotation);
                Instantiate(projectile, shotPoint2.position, rotation2);
                Instantiate(projectile, shotPoint3.position, rotation3);
                shotTime = Time.time + timeBetweenShots;
                anim.SetTrigger("Fire");
                cameraAnim.SetTrigger("shake");


            }
        }
    }
}
