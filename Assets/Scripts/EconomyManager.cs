using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private TMP_Text diamondAmountText;
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private TMP_Text levelText;

    [Header("HealthUpgradeStats")]
    [SerializeField] private TMP_Text heartCountText;
    [SerializeField] private TMP_Text heartUpgradeCostText;
    
    [Header("CoinBoostStats")]
    [SerializeField] private TMP_Text coinMultiplierText;
    [SerializeField] private TMP_Text coinBoostCostText;

    [SerializeField] private GameObject menuCanvas;

    void Awake()
    {
        
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
}
