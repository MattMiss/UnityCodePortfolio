using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, lifeTime;
    [SerializeField]
    private int damage;
    [SerializeField]
    private GameObject impactFX;
    [SerializeField]
    private bool damageEnemy, damagePlayer;

    // References
    private Rigidbody rBody;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GameManager.instance.gameIsPaused)
        {
            rBody.velocity = Vector3.zero;
        }
        else
        {
            rBody.velocity = transform.forward * moveSpeed;

            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blocker")
        {
            other.gameObject.GetComponent<Blocker>().RemoveAmount(1);
            DestroyBullet(); 
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(impactFX, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }
}
