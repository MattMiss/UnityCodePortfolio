using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncepad : MonoBehaviour
{
    [SerializeField]
    private float springForce;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CharacterControllerNew.instance.SpringUpward(springForce);
            AudioManager.instance.PlaySFX(0);
        }
    }
}
