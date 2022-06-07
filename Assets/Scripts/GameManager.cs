using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("About Health Variables")]
    private int health;
    public int collectedDiamonds;
    public int collectedCoins;

    public bool godMode;
    public bool gameStarted;

    [SerializeField] private float godModeDuration = 2f;
    [SerializeField] private float slowTimeDuration = 2f;

    [SerializeField] private GameObject healthObject;
    [SerializeField] private GameObject healthParentObject;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject inGameCanvas;
    [SerializeField] private GameObject finishCanvas;

    [Header("About Level Progress")]
    [SerializeField] private Transform finishTransform;
    [SerializeField] private LevelProgressBarManager levelProgressBar;
    [SerializeField] private CinemachineVirtualCamera startCam;
    [SerializeField] private CinemachineVirtualCamera inGameCam;

    [Header("About Collectables")]
    [SerializeField] private GameObject diamondSprite;
    [SerializeField] private GameObject godModeIcon;
    [SerializeField] private TMP_Text diamondCountText;
    [SerializeField] private TMP_Text coinCountText;

    private float levelLength;
    private float remainDistanceToFinish;

    private float godModeTimer;
    private float slowTimeTimer;

    private int obstacleCount = 10;
    private int diamondCount = 3;
    private int coinCount = 25;

    private bool slowTimeBooster;

    private GameObject[] spawnPoints;

    private PlayerController playerController;
    private ObjectPool objectPool;
    private EconomyManager economyManager;

    void Awake()
    {
        Instance = this;
        playerController = GetComponent<PlayerController>();
        objectPool = ObjectPool.Instance;
        economyManager = GameObject.FindGameObjectWithTag("EconomyManager").GetComponent<EconomyManager>();
        spawnPoints = GameObject.FindGameObjectsWithTag("ObstacleSpawn");

        levelLength = Vector3.Distance(transform.position, finishTransform.position);
        diamondCountText.text = "0";
        coinCountText.text = "0";        
        
    }

    private void Start()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            objectPool.SpawnFromPool("Obstacle", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.Euler(0,90,0));
        }

        for (int i = 0; i < diamondCount; i++)
        {
            objectPool.SpawnFromPool("Diamond", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
        
        for (int i = 0; i < coinCount; i++)
        {
            objectPool.SpawnFromPool("Coin", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }

        for (int i = 0; i < 2; i++)
        {
            objectPool.SpawnFromPool("TimeSlower", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            objectPool.SpawnFromPool("GodModeBooster", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }

    }

    void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            gameStarted = true;
            startCam.Priority = 0;

            menuCanvas.SetActive(false);
            inGameCanvas.SetActive(true);

            for (int i = 0; i < economyManager.heartCount; i++)
            {
                var heart = Instantiate(healthObject);
                heart.transform.parent = healthParentObject.transform;
            }
            health = economyManager.heartCount;
        }

        remainDistanceToFinish = Vector3.Distance(transform.position, finishTransform.position);
        levelProgressBar.SetProgress(1 - (remainDistanceToFinish / levelLength));

        if (godMode)
        {
            godModeTimer += Time.deltaTime;
            if (godModeTimer > godModeDuration)
            {
                godModeTimer = 0;
                godModeIcon.SetActive(false);
                godMode = false;
            }
        }

        if (slowTimeBooster)
        {
            slowTimeTimer += Time.deltaTime;
            if (slowTimeTimer >= slowTimeDuration)
            {
                Time.timeScale = 1;
                slowTimeTimer = 0;
                slowTimeBooster = false;
            }
            else
            {
                Time.timeScale = 0.5f;
            }
        }

        if (health <= 0)
        {
            LevelFailed();
        }
    }

    void LevelFailed()
    {
        gameStarted = false;
        playerController.isGameStarted = false;

        GetComponent<Animator>().SetTrigger("Failed");
        startCam.Priority = 10;
        StartCoroutine(WaitForFailCanvas());

        inGameCanvas.SetActive(false);        
    }
    void LevelPassed()
    {
        gameStarted = false;
        playerController.isGameStarted = false;
        inGameCanvas.SetActive(false);       

        StartCoroutine(WaitForPassedCanvas());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Diamond"))
        {
            other.gameObject.SetActive(false);
            collectedDiamonds++;

            diamondCountText.text = collectedDiamonds.ToString();
            diamondCount--;

            while (diamondCount < 3)
            {
                var selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                //if (remainDistanceToFinish < 15f)
                //{
                //    diamondCount = 3;
                //}
                if (selectedSpawn.z - transform.position.z > 10 || remainDistanceToFinish < 25f)
                {
                    objectPool.SpawnFromPool("Diamond", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f,0), Quaternion.identity);
                    diamondCount++;
                }
            }

            diamondSprite.GetComponent<Animator>().SetTrigger("DiamondCollected");
            objectPool.diamondCollectedFX.transform.position = other.transform.position;
            objectPool.diamondCollectedFX.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            collectedCoins += economyManager.coinMultiplier;
            coinCount--;

            while (coinCount < 20)
            {
                var selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                //if (remainDistanceToFinish < 15f)
                //{
                //    coinCount = 20;
                //}
                if (selectedSpawn.z - transform.position.z > 10 || remainDistanceToFinish < 25f)
                {
                    objectPool.SpawnFromPool("Coin", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                    coinCount++;
                }
            }
            coinCountText.text = collectedCoins.ToString();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
            obstacleCount--;

            if (!godMode)
            {
                health--;
                Destroy(healthParentObject.transform.GetChild(healthParentObject.transform.childCount - 1).gameObject);
            }

            while (obstacleCount < 10)
            {
                var selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

                //if (remainDistanceToFinish < 15f)
                //{
                //    obstacleCount = 10;
                //}
                if (selectedSpawn.z - transform.position.z > 10 || remainDistanceToFinish < 25f)
                {
                    objectPool.SpawnFromPool("Obstacle", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.Euler(0, 90, 0));
                    obstacleCount++;
                }
            }            

            if (health > 0)
            {
                playerController.GetHitted();
                godMode = true;
            }
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            startCam.Priority = 10;
            playerController.Finished();
            LevelPassed();
        }
        else if (other.gameObject.CompareTag("TimeSlower"))
        {
            slowTimeBooster = true;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("GodMode"))
        {
            godMode = true;
            godModeIcon.SetActive(true);
            other.gameObject.SetActive(false);
        }
    }

    IEnumerator WaitForFailCanvas()
    {
        yield return new WaitForSeconds(2);
        finishCanvas.SetActive(true);
        finishCanvas.transform.GetChild(0).gameObject.SetActive(true);
    }
    
    IEnumerator WaitForPassedCanvas()
    {
        yield return new WaitForSeconds(2);
        finishCanvas.SetActive(true);
        finishCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }
}
