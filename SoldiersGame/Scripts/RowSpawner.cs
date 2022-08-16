using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSpawner : MonoBehaviour
{
    public static RowSpawner instance;

    [SerializeField] private SingleRow singleRow;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float rowSpeed;

    private float timeLeft;

    void Awake()
    {
        instance = this;
    }

    public void StartSpawning()
    {
        SpawnRow();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameIsPaused) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            SpawnRow();
        }
    }

    void SpawnRow()
    {
        SingleRow row = Instantiate(singleRow, transform.position, Quaternion.identity);
        row.InitRow(rowSpeed, GetRowAmounts());
        timeLeft = timeBetweenSpawns;
    }

    int[] GetRowAmounts()
    {
        int[] rowAmounts = new int[6];

        //int randomEmpty = Random.Range(0, 5);

        for(int i = 0; i < rowAmounts.Length; i++)
        {
            /*
            if (i == randomEmpty)
            {
                rowAmounts[i] = 0;
            }
            else{
                rowAmounts[i] = Random.Range(1, 9);
            }
            */
            rowAmounts[i] = Random.Range(1, 9);
        }

        return rowAmounts;
    }
}
