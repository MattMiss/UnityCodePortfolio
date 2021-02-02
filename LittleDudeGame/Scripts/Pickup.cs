using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public Weapon weaponToEquip;

    public GameObject effect;

    public GameObject pickupText;

    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().refreshPickups();
    }

    // Show Pickup text when inside collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pickupText.SetActive(true);
        }
    }

    // Remoev pickup text when exiting collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pickupText.SetActive(false);
        }
    }

    public void SwapWeapon()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ChangeWeapon(weaponToEquip);
        Destroy(gameObject);
    }

}
