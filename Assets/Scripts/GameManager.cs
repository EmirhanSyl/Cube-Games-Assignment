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

    [SerializeField] private int totalObstacleCount = 10;
    [SerializeField] private int totalDiamondCount = 3;
    [SerializeField] private int totalCoinCount = 25;
    [SerializeField] private int totalBoosterCount = 2;

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
    [SerializeField] private TMP_Text levelText;

    private float levelLength;
    private float remainDistanceToFinish;

    private float godModeTimer;
    private float slowTimeTimer;

    private int obstacleCount;
    private int diamondCount;
    private int coinCount;

    private bool slowTimeBooster;
    private bool failBool;

    private GameObject[] spawnPoints;

    private PlayerController playerController;
    private ObjectPool objectPool;
    private EconomyManager economyManager;
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        objectPool = ObjectPool.Instance;
        economyManager = GameObject.FindGameObjectWithTag("EconomyManager").GetComponent<EconomyManager>();
        spawnPoints = GameObject.FindGameObjectsWithTag("ObstacleSpawn");

        levelLength = Vector3.Distance(transform.position, finishTransform.position);
        diamondCountText.text = "0";
        coinCountText.text = "0";

        obstacleCount = totalObstacleCount;
        diamondCount = totalDiamondCount;
        coinCount = totalCoinCount;
        
    }

    private void Start()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            ObjectPool.Instance.SpawnFromPool("Obstacle", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.Euler(0,90,0));
        }

        for (int i = 0; i < diamondCount; i++)
        {
            ObjectPool.Instance.SpawnFromPool("Diamond", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
        
        for (int i = 0; i < coinCount; i++)
        {
            ObjectPool.Instance.SpawnFromPool("Coin", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }

        for (int i = 0; i < totalBoosterCount; i++)
        {
            ObjectPool.Instance.SpawnFromPool("TimeSlower", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            ObjectPool.Instance.SpawnFromPool("GodModeBooster", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
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
            levelText.text = economyManager.level.ToString();
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

        if (!failBool)
        {
            playerController.hittedParticles.gameObject.SetActive(true);
            playerController.hittedParticles.Play();
            failBool = true;
        }

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

            while (diamondCount < totalDiamondCount)
            {
                var selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                //if (remainDistanceToFinish < 15f)
                //{
                //    diamondCount = 3;
                //}
                if (selectedSpawn.z - transform.position.z > 10 || remainDistanceToFinish < 25f)
                {
                    ObjectPool.Instance.SpawnFromPool("Diamond", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f,0), Quaternion.identity);
                    diamondCount++;
                }
            }

            diamondSprite.GetComponent<Animator>().SetTrigger("DiamondCollected");
            ObjectPool.Instance.diamondCollectedFX.transform.position = other.transform.position;
            ObjectPool.Instance.diamondCollectedFX.SetActive(true);
            ObjectPool.Instance.diamondCollectedFX.GetComponent<ParticleSystem>().Play();
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            collectedCoins += economyManager.coinMultiplier;
            coinCount--;
            audioSource.Play();

            while (coinCount < totalCoinCount)
            {
                var selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                //if (remainDistanceToFinish < 15f)
                //{
                //    coinCount = 20;
                //}
                if (selectedSpawn.z - transform.position.z > 10 || remainDistanceToFinish < 25f)
                {
                    ObjectPool.Instance.SpawnFromPool("Coin", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
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

            while (obstacleCount < totalObstacleCount)
            {
                var selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

                //if (remainDistanceToFinish < 15f)
                //{
                //    obstacleCount = 10;
                //}
                if (selectedSpawn.z - transform.position.z > 10 || remainDistanceToFinish < 25f)
                {
                    ObjectPool.Instance.SpawnFromPool("Obstacle", spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.Euler(0, 90, 0));
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
