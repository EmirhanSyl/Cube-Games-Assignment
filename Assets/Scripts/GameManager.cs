using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("About Health Variables")]
    public int health;
    public bool godMode;

    public bool gameStarted;

    [SerializeField] private float godModeDuration = 2f;

    [SerializeField] private GameObject healthObject;
    [SerializeField] private GameObject healthParentObject;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject inGameCanvas;

    [Header("About Level Progress")]
    [SerializeField] private Transform finishTransform;
    [SerializeField] private LevelProgressBarManager levelProgressBar;
    [SerializeField] private CinemachineVirtualCamera startCam;
    [SerializeField] private CinemachineVirtualCamera inGameCam;

    [Header("About Collectables")]
    [SerializeField] private GameObject diamondSprite;
    [SerializeField] private TMP_Text diamondCountText;
    [SerializeField] private TMP_Text coinCountText;

    private float levelLength;
    private float remainDistanceToFinish;

    private float godModeTimer;

    private int collectedDiamonds;
    private int collectedCoins;

    private PlayerController playerController;
    private ObjectPool objectPool;
    private EconomyManager economyManager;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        objectPool = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectPool>();
        economyManager = GameObject.FindGameObjectWithTag("EconomyManager").GetComponent<EconomyManager>();

        levelLength = Vector3.Distance(transform.position, finishTransform.position);
        diamondCountText.text = "0";
        coinCountText.text = "0";

        //for (int i = 0; i < health; i++)
        //{
        //    var heart = Instantiate(healthObject);
        //    heart.transform.parent = healthParentObject.transform;
        //}
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
        }

        remainDistanceToFinish = Vector3.Distance(transform.position, finishTransform.position);
        levelProgressBar.SetProgress(1 - (remainDistanceToFinish / levelLength));

        if (godMode)
        {
            godModeTimer += Time.deltaTime;
            if (godModeTimer > godModeDuration)
            {
                godModeTimer = 0;
                godMode = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Diamond"))
        {
            other.gameObject.SetActive(false);
            collectedDiamonds++;

            diamondCountText.text = collectedDiamonds.ToString();

            diamondSprite.GetComponent<Animator>().SetTrigger("DiamondCollected");
            objectPool.diamondCollectedFX.transform.position = other.transform.position;
            objectPool.diamondCollectedFX.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            collectedCoins += economyManager.coinMultiplier;

            coinCountText.text = collectedCoins.ToString();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
            if (!godMode)
            {
                health--;
                healthParentObject.transform.GetChild(healthParentObject.transform.childCount - 1).gameObject.SetActive(false);
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
        }
    }
}