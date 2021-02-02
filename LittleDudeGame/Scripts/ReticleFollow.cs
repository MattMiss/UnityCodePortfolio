using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleFollow : MonoBehaviour {

    private void Start()
    {
        Cursor.visible = false;
    }
    void Update ()
    {
        transform.position = Input.mousePosition;
	}
}
