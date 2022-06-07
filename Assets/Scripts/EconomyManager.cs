using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public int diamondAmount;
    public int coinAmount;

    public int level = 1;

    public int heartCount = 1;
    public int heartUpgradeCost = 10;

    public int coinMultiplier = 1;
    public int coinBoostCost = 100;

    [SerializeField] private float lostTextAnimDuration = 0.4f;

    [SerializeField] private TMP_Text diamondAmountText;
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private TMP_Text levelText;

    [Header("HealthUpgradeStats")]
    [SerializeField] private TMP_Text heartCountText;
    [SerializeField] private TMP_Text heartUpgradeCostText;
    
    [Header("CoinBoostStats")]
    [SerializeField] private TMP_Text coinMultiplierText;
    [SerializeField] private TMP_Text coinBoostCostText;


    [Header("LevelFinishVariables")]
    //Level Failed
    [SerializeField] private GameObject lostCoinsBG;
    [SerializeField] private GameObject lostDiamondsBG;

    [SerializeField] private TMP_Text lostCoinsText;
    [SerializeField] private TMP_Text lostDiamondsText;


    [Space(5)]
    //Level Passed
    [SerializeField] private GameObject earnedCoinsBG;
    [SerializeField] private GameObject earnedDiamondsBG;

    [SerializeField] private GameObject totalCoinsIcon;
    [SerializeField] private GameObject totalDiamondsIcon;

    [SerializeField] private TMP_Text earnedCoinsText;
    [SerializeField] private TMP_Text earnedDiamondsText;
    [SerializeField] private TMP_Text totalCoinsText;
    [SerializeField] private TMP_Text totalDiamondsText;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject finishCanvas;

    private float lostTextAnimTimer;

    private int diamondAnimCount;
    private int coinAnimCount;

    private bool retryButtonActive;
    private bool nextLevelButtonPressed;

    private GameManager gameManager;

    void Awake()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
            diamondAmount = PlayerPrefs.GetInt("DiamondAmount");
            coinAmount = PlayerPrefs.GetInt("CoinAmount");
        }
        else
        {
            level = 1;
            diamondAmount = 0;
            coinAmount = 0;
            PlayerPrefs.SetInt("Level", level);
        }
        if (level > 3)
        {
            level = 3;
        }

        if (PlayerPrefs.HasKey("HealthLevel"))
        {
            heartCount = PlayerPrefs.GetInt("HealthLevel");
        }

        if (PlayerPrefs.HasKey("CoinBoostLevel"))
        {
            coinMultiplier = PlayerPrefs.GetInt("CoinBoostLevel");
        }

        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
    }

    
    void Update()
    {
        if (menuCanvas.activeSelf)
        {
            diamondAmountText.text = diamondAmount.ToString();
            coinAmountText.text = coinAmount.ToString();
            levelText.text = level.ToString();

            heartCountText.text = "x" + heartCount.ToString();
            coinMultiplierText.text = "x" + coinMultiplier.ToString();

            heartUpgradeCostText.text = (heartUpgradeCost * heartCount).ToString();
            coinBoostCostText.text = (coinBoostCost * coinMultiplier).ToString();
        }
        if (finishCanvas.activeSelf)
        {
            if (finishCanvas.transform.GetChild(0).gameObject.activeSelf)
            {
                if (!retryButtonActive)
                {
                    coinAnimCount = gameManager.collectedCoins;
                    diamondAnimCount = gameManager.collectedDiamonds;

                    lostCoinsText.text = "x" + coinAnimCount.ToString();
                    lostDiamondsText.text = "x" + diamondAnimCount.ToString();
                }
                else
                {
                    lostTextAnimTimer += Time.deltaTime;
                    if (lostTextAnimTimer >= lostTextAnimDuration)
                    {
                        if (diamondAnimCount > 0)
                        {
                            diamondAnimCount--;
                            lostDiamondsBG.GetComponent<Animator>().SetTrigger("Triggered");
                        }
                        if (coinAnimCount > 0)
                        {
                            coinAnimCount--;
                            lostCoinsBG.GetComponent<Animator>().SetTrigger("Triggered");
                        }
                        lostCoinsText.text = "x" + coinAnimCount.ToString();
                        lostDiamondsText.text = "x" + diamondAnimCount.ToString();
                        lostTextAnimTimer = 0;

                        if (diamondAnimCount == 0 && coinAnimCount == 0)
                        {
                            StartCoroutine(WaitForNextScene());
                        }
                    }
                }
            }
            else if (finishCanvas.transform.GetChild(1).gameObject.activeSelf)
            {
                if (!nextLevelButtonPressed)
                {
                    coinAnimCount = gameManager.collectedCoins;
                    diamondAnimCount = gameManager.collectedDiamonds;

                    earnedCoinsText.text = "x" + coinAnimCount.ToString();
                    earnedDiamondsText.text = "x" + diamondAnimCount.ToString();

                    totalCoinsText.text = coinAmount.ToString();
                    totalDiamondsText.text = diamondAmount.ToString();
                }
                else
                {
                    lostTextAnimTimer += Time.deltaTime;
                    if (lostTextAnimTimer >= lostTextAnimDuration)
                    {
                        if (diamondAnimCount > 0)
                        {
                            diamondAmount++;
                            diamondAnimCount--;
                            earnedDiamondsBG.GetComponent<Animator>().SetTrigger("PassedTrigger");
                            totalDiamondsIcon.GetComponent<Animator>().SetTrigger("Triggered");
                        }
                        if (coinAnimCount > 0)
                        {
                            coinAmount++;
                            coinAnimCount--;
                            earnedCoinsBG.GetComponent<Animator>().SetTrigger("PassedTrigger");
                            totalCoinsIcon.GetComponent<Animator>().SetTrigger("Triggered");
                        }
                        earnedCoinsText.text = "x" + coinAnimCount.ToString();
                        earnedDiamondsText.text = "x" + diamondAnimCount.ToString();

                        totalCoinsText.text = coinAmount.ToString();
                        totalDiamondsText.text = diamondAmount.ToString();
                        lostTextAnimTimer = 0;

                        if (diamondAnimCount == 0 && coinAnimCount == 0)
                        {
                            PlayerPrefs.SetInt("DiamondAmount", diamondAmount);
                            PlayerPrefs.SetInt("CoinAmount", coinAmount);
                            StartCoroutine(WaitForNextScene());
                        }
                    }
                }
            }
        }
    }

    public void UpgradeHealthButton()
    {
        var levelCost = heartUpgradeCost * heartCount;

        if (diamondAmount < levelCost || heartCount >= 4)
        {
            return;
        }

        heartCount++;
        diamondAmount -= levelCost;

        PlayerPrefs.SetInt("HealthLevel", heartCount);
        PlayerPrefs.SetInt("DiamondAmount", diamondAmount);
    }

    public void UpgradeCoinBoosterButton()
    {
        var levelCost = coinBoostCost * coinMultiplier;

        if (coinAmount < levelCost)
        {
            return;
        }

        coinMultiplier++;
        coinAmount -= levelCost;

        PlayerPrefs.SetInt("CoinBoostLevel", coinMultiplier);
        PlayerPrefs.SetInt("CoinAmount", coinAmount);
    }

    public void RetryButton()
    {
        retryButtonActive = true;
    }

    public void NextLevelButton()
    {
        level++;
        if (level > 3)
        {
            level = 3;
        }
        PlayerPrefs.SetInt("Level", level);
        
        nextLevelButtonPressed = true;
    }

    public void RestartPreferances()
    {
        PlayerPrefs.DeleteAll();
    }

    IEnumerator WaitForNextScene()
    {        
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(level);
    }

}
