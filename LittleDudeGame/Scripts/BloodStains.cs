using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStains : MonoBehaviour {

    public Sprite[] bloodSprites;
    private SpriteRenderer rend;
    private Animator anim;

    public float lifeTime;

    void Start ()
    {
        int randNum = Random.Range(0, 3);
        float randSize = Random.Range(.8f, 1);

        anim = GetComponent<Animator>();
        rend = GetComponentInChildren<SpriteRenderer>();

        rend.sprite = bloodSprites[randNum];
        transform.localScale *= new Vector2(randSize, randSize);

        Invoke("FadeBlood", lifeTime);
    }


    void FadeBlood()
    {
        anim.SetTrigger("fade");
    }

    void DestoryBlood()
    {
        Destroy(gameObject);
    }
}
