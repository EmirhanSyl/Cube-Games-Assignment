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
    [SerializeField] private GameObject lostCoinsBG;
    [SerializeField] private GameObject lostDiamondsBG;

    [SerializeField] private TMP_Text lostCoinsText;
    [SerializeField] private TMP_Text lostDiamondsText;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject finishCanvas;

    private float lostTextAnimTimer;

    private int diamondAnimCount;
    private int coinAnimCount;

    private bool retryButtonActive;

    private GameManager gameManager;

    void Awake()
    {
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
    }

    public void RetryButton()
    {
        retryButtonActive = true;
    }

    IEnumerator WaitForNextScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
