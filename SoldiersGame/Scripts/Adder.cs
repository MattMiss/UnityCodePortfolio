using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Adder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;
    private int amount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAmount(int num)
    {
        amount = num;
        icon.sprite = PlusMinusIconChooser.instance.GetIcon(PlusMinusIconChooser.IconType.Plus, num);
    }


    void OnTriggerEnter(Collider other)
    {   
        //Debug.Log("Triggered Adder!");

        if (other.tag == "Player")
        {
            PlayerController.instance.AddPlayers(amount);
            Destroy(gameObject);
        }
    }
}
