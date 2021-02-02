using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBar : MonoBehaviour
{
    private CraftMenuMainLayer craftMenuMainLayer;

    // Start is called before the first frame update
    void Start()
    {
        craftMenuMainLayer = GetComponentInParent<CraftMenuMainLayer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, craftMenuMainLayer.GetAngleFromCenter());
    }
}
