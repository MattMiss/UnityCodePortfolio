using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int health;

    [HideInInspector]
    public Transform player;

    public float speed;
    public float normalSpeed;

    public float timeBetweenAttacks;

    public int damage;

    public int pickupChance;
    public GameObject[] pickups;

    public int healthPickupChance;
    public GameObject healthPickup;

    public GameObject deathEffect;

    public GameObject bloodStain;

    public float startDazedTime;

    [HideInInspector]
    public float dazedTime;

    private SpriteRenderer[] bodyParts;
    public Color hurtColor;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bodyParts = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(Flash());
        dazedTime = startDazedTime;
        health -= damageAmount;

        if (health <= 0)
        {
            int randomNumber = Random.Range(0, 101);
            if (randomNumber < pickupChance)
            {
                GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
                Instantiate(randomPickup, transform.position, transform.rotation);
            }

            int randHealth = Random.Range(0, 101);
            if (randHealth < healthPickupChance)
            {
                Instantiate(healthPickup, transform.position, transform.rotation);
            }

            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Instantiate(bloodStain, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Destroy(gameObject);
        }
    }

    IEnumerator Flash()
    { 
        for(int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].color = hurtColor;
        }
        yield return new WaitForSeconds(.05f);
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].color = Color.white;
        }
    }
}
