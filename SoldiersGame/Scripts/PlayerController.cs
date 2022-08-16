using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform[] fireOrigins;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject playerSpot;
    [SerializeField] private GameObject playerDestroyFX;
    private List<GameObject> playerSpots = new List<GameObject>();

    [SerializeField] private int livesLeft = 0;
    [SerializeField] private int initialLives = 16;
    [SerializeField] private float addOrLoseTimeDelay = .2f;
    [SerializeField] private float firingInterval = .3f;

    [SerializeField] private float minX = -.1f;
    [SerializeField] private float maxX = 5f;

    private float timeSinceLoss = 0f;
    private float timeSinceAdd = 0f;
    private float fireCounter;
    private float fireTypeResetCounter = -1f;
    private float fireTypeResetLength;
    private PowerupIconChooser.PowerupType currentFireType;

    public enum FireType
    {
        SingleShot,
        TripleVertical,
        TripleSpread
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIController.instance.SetLivesText(livesLeft);
        //SpawnPlayers(30);
        fireCounter = firingInterval;

        timeSinceAdd = addOrLoseTimeDelay;
        currentFireType = PowerupIconChooser.PowerupType.RegularShot;
        PowerupIconSlider.instance.SetSliderAmount(1f);
        SpawnPlayerSpots();
        AddPlayers(initialLives);
    }

    void Update()
    {
        if (GameManager.instance.gameIsPaused) return;

        if (Input.GetKey(KeyCode.A) && transform.position.x > minX)
        {
            transform.position += Vector3.left * Time.deltaTime * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.D) && transform.position.x < maxX)
        {
            transform.position += Vector3.right * Time.deltaTime * movementSpeed;
        }

        timeSinceLoss += Time.deltaTime;
        timeSinceAdd += Time.deltaTime;
        fireCounter -= Time.deltaTime;

        fireTypeResetCounter -= Time.deltaTime;

        if (currentFireType != PowerupIconChooser.PowerupType.RegularShot)
        {
            if (fireTypeResetCounter < 0)
            {
                currentFireType = PowerupIconChooser.PowerupType.RegularShot;
                
                PowerupIconSlider.instance.SetPowerupIcon(PowerupIconChooser.instance.GetIcon(currentFireType));
            }
            PowerupIconSlider.instance.SetSliderAmount(fireTypeResetCounter / fireTypeResetLength);
        }  
        else
        {
            PowerupIconSlider.instance.SetSliderAmount(1f);
        }         

        ContinueFiring();
    }

    private void PlayerLost()
    {
        Debug.Log("Player has lost!");
        GameManager.instance.GameOver();
    }


    private void ContinueFiring()
    {
        if (fireCounter <= 0 && livesLeft > 0)
        {
            FireShots();
            fireCounter = firingInterval;
        }
    }

    private void FireShots()
    {
       
        switch(currentFireType)
        {
            case PowerupIconChooser.PowerupType.RegularShot:
                Instantiate(bullet, fireOrigins[1].position, fireOrigins[1].rotation);
                break;
            case PowerupIconChooser.PowerupType.TripleShotVertical:
                Instantiate(bullet, fireOrigins[0].position, fireOrigins[0].rotation);
                Instantiate(bullet, fireOrigins[1].position, fireOrigins[1].rotation);
                Instantiate(bullet, fireOrigins[2].position, fireOrigins[2].rotation);
                break;
            case PowerupIconChooser.PowerupType.TripleShotSpread:
                Instantiate(bullet, fireOrigins[1].position, fireOrigins[1].rotation);
                Instantiate(bullet, fireOrigins[3].position, fireOrigins[3].rotation);
                Instantiate(bullet, fireOrigins[4].position, fireOrigins[4].rotation);
                break;
        }
    }

    private void SpawnPlayerSpots()
    {
        GameObject last = gameObject;
        GameObject initial = Instantiate(playerSpot, transform.position, Quaternion.identity);
        initial.transform.parent = transform;
        playerSpots.Add(initial);
        for (int i = 0; i < 19; i++)
        {
            
            for (int j = 0; j < 3; j++)
            {
                GameObject spot = Instantiate(playerSpot, transform.position + new Vector3((j * .3f) - .3f, 0f, (i + 1) * -.5f), Quaternion.identity);
                if (i == 0)
                {
                    spot.transform.parent = transform;
                }
                else
                {
                    spot.GetComponent<PlayerSpot>().targetParent = playerSpots[(((i - 1) * 3) + j) + 1];
                }
                playerSpots.Add(spot);
            }
        }
    }


    public void AddPlayers(int playerCount)
    {
        Debug.Log("Adding Players: " + playerCount);
        if (timeSinceAdd >= addOrLoseTimeDelay)
        {
            timeSinceAdd = 0;
            int playersAdded = 0;
            foreach(GameObject spot in playerSpots)
            {
                if (playersAdded == playerCount) return;

                PlayerSpot ps = spot.GetComponent<PlayerSpot>();
                if (ps && !ps.isFilled)
                {
                    ps.ShowPlayer(true);
                    playersAdded++;
                    livesLeft++;
                    ps.isFilled = true;
                    Debug.Log("Lives Left: " + livesLeft);
                    UIController.instance.SetLivesText(livesLeft);
                }
            }
            
            
        }
    }

    public void RemovePlayers(int playerCount)
    {
        if (timeSinceLoss >= addOrLoseTimeDelay)
        {
            timeSinceLoss = 0;
            int playersRemoved = 0;

            for(int i = playerSpots.Count - 1; i >= 0; i--)
            {
                if (playersRemoved == playerCount) return;

                PlayerSpot ps = playerSpots[i].GetComponent<PlayerSpot>();
                if (ps && ps.isFilled)
                {
                    ps.ShowPlayer(false);
                    Instantiate(playerDestroyFX, playerSpots[i].transform.position, playerSpots[i].transform.rotation);
                    playersRemoved++;
                    livesLeft--;
                    UIController.instance.SetLivesText(livesLeft);
                    ps.isFilled = false;
                    if (i == 0)
                    {
                        PlayerLost();
                    }
                }
            }
            
        }
    }

    public void ChangeFireType(PowerupIconChooser.PowerupType fireType, float length)
    {
        currentFireType = fireType;

        fireTypeResetCounter = length;
        fireTypeResetLength = length;

        PowerupIconSlider.instance.SetPowerupIcon(PowerupIconChooser.instance.GetIcon(fireType));
    }

    public bool IsAlive()
    {
        return livesLeft > 0;
    }


}
