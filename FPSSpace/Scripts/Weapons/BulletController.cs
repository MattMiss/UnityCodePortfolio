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
        rBody.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        else if (other.gameObject.tag == "HeadShot" && damageEnemy)
        {
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 2);
        }
        else if(other.gameObject.tag == "Player" && damagePlayer)
        {
            // Damage player
            PlayerHealthController.instance.DamagePlayer(damage);
        }
        else if (other.gameObject.tag == "Player")
        {   
            return;
        }
        DestroyBullet(); 

    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
        Instantiate(impactFX, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }
}
