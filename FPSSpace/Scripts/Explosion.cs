using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private int damage = 25;
    [SerializeField]
    private bool damageEnemy, damagePlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        else if(other.gameObject.tag == "Player" && damagePlayer)
        {
            // Damage player
            PlayerHealthController.instance.DamagePlayer(damage);
        }
        Destroy(gameObject, 2f);
    }
}
