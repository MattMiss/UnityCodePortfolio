using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer top, side, front;
    [SerializeField]
    private Light pointLight;

    public void SetFlashColor(Color color)
    {
        top.color = color;
        side.color = color;
        front.color = color;
        pointLight.color = color;
    }
}
