using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blocker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private GameObject explodeFX;
    private int amount;


    public void SetAmount(int num)
    {
        amount = num;
        if (amount == 0)
        {
            Explode();
        }
        else{
            icon.sprite = PlusMinusIconChooser.instance.GetIcon(PlusMinusIconChooser.IconType.Minus, num);
        }
    }

    public void RemoveAmount(int num)
    {
        SetAmount(Mathf.Clamp(amount - num, 0, amount));
    }


    void OnTriggerEnter(Collider other)
    {   
        //Debug.Log("Triggered Blocker!");

        if (other.tag == "Player")
        {
            PlayerController.instance.RemovePlayers(amount);
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explodeFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
