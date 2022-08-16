using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{

    [SerializeField]
    private int currentHealth = 5;

    [SerializeField]
    private EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (enemyAI != null)
        {
            enemyAI.WasShot();
        }

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX(2);
            Die();
        }
    }

    void Die()
    {
        enemyAI.EnemyDied();
    }
}
