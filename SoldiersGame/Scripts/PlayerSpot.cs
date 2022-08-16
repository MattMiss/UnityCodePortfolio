using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpot : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    public GameObject targetParent;

    public bool isFilled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetParent)
        {
            Vector3 newPosition =  transform.position;
            newPosition.x =  targetParent.transform.position.x;
            transform.position = Vector3.Lerp(transform.position, newPosition, 10f * Time.deltaTime);
        }
    }

    public void ShowPlayer(bool show)
    {
        playerObject.SetActive(show);
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, .1f);
    }
}
