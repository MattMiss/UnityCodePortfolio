using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float speed;

    public int health;

    private Rigidbody2D rb;
    private Animator anim;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private Vector2 moveAmount;

    public Animator hurtAnim;

    private GameObject[] availablePickups;
    private GameObject closestPickup;
    private float closestDistance = 8f;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	 void Update ()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        // Print the list of pickup names in game
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < availablePickups.Length; i++)
            {
                Debug.Log("Available Pickups: " + availablePickups[i]);
                Debug.Log("Closest Pickup: " + closestPickup);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            refreshPickups();   // Check to find "pickup" GameObjects in scene so you don't get a null in the array

            for (int i = 0; i < availablePickups.Length; i++)
            {
                if (Vector2.Distance(availablePickups[i].transform.position, transform.position) < 8)
                {
                    if (Vector2.Distance(availablePickups[i].transform.position, transform.position) < closestDistance)
                    {
                        closestDistance = Vector2.Distance(availablePickups[i].transform.position, transform.position);
                        closestPickup = availablePickups[i]; 
                    }
                }
            }
            ChangeWeapon(closestPickup.GetComponent<Pickup>().weaponToEquip);
            closestPickup.GetComponent<Pickup>().SwapWeapon();
            closestDistance = 8f;
        }
    }

     void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime); 
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        UpdateHealthUI(health);
        hurtAnim.SetTrigger("hurt");

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeWeapon(Weapon weaponToEquip)
    {
        Destroy(GameObject.FindGameObjectWithTag("Weapon"));
        Instantiate(weaponToEquip, transform.position, transform.rotation, transform);
    }

    void UpdateHealthUI( int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void Heal(int healAmount)
    {
        if (health + healAmount > 5)
        {
            health = 5;
        }
        else
        {
            health += healAmount;
        }
        UpdateHealthUI(health);
    }

    public void refreshPickups()
    {
        availablePickups = GameObject.FindGameObjectsWithTag("Pickup");
    }

}
