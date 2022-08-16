using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRow : MonoBehaviour
{
    [SerializeField] private Blocker blocker;
    [SerializeField] private Adder adder;
    [SerializeField] private TripleShotVertical tripleShotVertical;
    [SerializeField] private TripleShotSpread tripleShotSpread;
    [SerializeField] private int blockerCount;
    [SerializeField] private float blockerOffset;
    [SerializeField] private float rowSpeed;

    private int[] allBlockAmounts;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameIsPaused) return;
        
        float step = rowSpeed * Time.deltaTime;
        transform.position -= transform.forward * step;       
    }

    void SpawnBlockers()
    {
        for (int i = 0; i < blockerCount; i++)
        {
            Vector3 spawnLoc = transform.position;
            spawnLoc.x = i * blockerOffset;
            if (allBlockAmounts[i] != 0)
            {
                Blocker blockerObject = Instantiate(blocker, spawnLoc, Quaternion.identity);
                blockerObject.SetAmount(allBlockAmounts[i]);
                blockerObject.transform.parent = transform;
            }
            else 
            {
                float spawnChance = Random.Range(0, 100);
                if (spawnChance > 60)
                {
                    if (spawnChance <= 80)
                    {
                        Adder addObject = Instantiate(adder, spawnLoc, Quaternion.identity);
                        addObject.SetAmount(Random.Range(1, 6));
                        addObject.transform.parent = transform;
                    }
                    else if (spawnChance <= 90)
                    {
                        TripleShotVertical tripleShotObject = Instantiate(tripleShotVertical, spawnLoc, Quaternion.identity);
                        tripleShotObject.transform.parent = transform;
                    }
                    else
                    {
                        TripleShotSpread tripleShotObject = Instantiate(tripleShotSpread, spawnLoc, Quaternion.identity);
                        tripleShotObject.transform.parent = transform;
                    }
                }
            }
        }
    }

    public void InitRow(float speed, int[] blockAmounts)
    {
        rowSpeed = speed;
        allBlockAmounts = blockAmounts;

        SpawnBlockers();
    }
}
