using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    
    public GameObject projectile;
    public Transform shotPoint;
    public float timeBetweenShots;

    [HideInInspector]
    public float shotTime;
    [HideInInspector]
    public bool isFlipped = false;

    [HideInInspector]
    public Animator anim;

    [HideInInspector]
    public Animator cameraAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        cameraAnim = Camera.main.GetComponent<Animator>();
    }

    public virtual void Update ()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        // Check if the gun is pointing left, then flip the sprite on the y axis
        Vector3 theScale = transform.localScale;

        if ((rotation.z > .7 || rotation.z < -.7) && isFlipped == false)
        {  
            theScale.y *= -1;
            isFlipped = true;
        }

        else if ((rotation.z < .7 && rotation.z > -.7) && isFlipped == true)
        {
            theScale.y *= -1;
            isFlipped = false;
        }

        transform.localScale = theScale;

        Shoot();
    }

    public virtual void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= shotTime)
            {
                Instantiate(projectile, shotPoint.position, transform.rotation);
                shotTime = Time.time + timeBetweenShots;
                anim.SetTrigger("Fire");
                cameraAnim.SetTrigger("shake");
            }
        }
    }
}
