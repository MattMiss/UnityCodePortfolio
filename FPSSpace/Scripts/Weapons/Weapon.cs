using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] float fireRate = .2f, burstRate = 2f;
    [SerializeField] int burstShotCount = 3;
    [SerializeField] private Animator anim;

    private float nextShootTime;
    private float nextBurstTime;
    private int burstCount = 0;

    public Transform muzzlePoint;

    public void AimAtPlayer()
    {
        muzzlePoint.LookAt(CharacterControllerNew.instance.GetPosition() + new Vector3(0f, Random.Range(0f, .3f), 0f));
    }

    public void FireWeapon()
    {
        Debug.Log("firing Weapon");
        if (Time.time > nextShootTime && Time.time > nextBurstTime)
        {
            anim.SetTrigger("fireShot");
            StartCoroutine(SendBullet());
            nextShootTime = Time.time + fireRate;
            burstCount++;
        }
        if (burstCount >= burstShotCount)
        {
            nextBurstTime = Time.time + burstRate;
            burstCount = 0;
        }
    }

    private IEnumerator SendBullet()
    {
        yield return new WaitForSeconds(.04f);

        Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
    }
}
