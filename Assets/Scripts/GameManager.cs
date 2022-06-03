using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("About Health Variables")]
    public int health;
    public bool godMode;

    [SerializeField] private float godModeDuration = 2f;

    [SerializeField] private GameObject healthObject;
    [SerializeField] private GameObject healthParentObject;

    [Header("About Level Progress")]
    [SerializeField] private Transform finishTransform;
    [SerializeField] private LevelProgressBarManager levelProgressBar;

    [Header("About Collectables")]
    [SerializeField] private TMP_Text diamondCountText;
    [SerializeField] private TMP_Text coinCountText;

    private float levelLength;
    private float remainDistanceToFinish;

    private float godModeTimer;

    private int collectedDiamonds;
    private int collectedCoins;

    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();

        levelLength = Vector3.Distance(transform.position, finishTransform.position);
        diamondCountText.text = "0";
        coinCountText.text = "0";

        for (int i = 0; i < health; i++)
        {
            var heart = Instantiate(healthObject);
            heart.transform.parent = healthParentObject.transform;
        }
    }

    
    void Update()
    {
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
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            collectedCoins++;

            coinCountText.text = collectedCoins.ToString();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.SetActive(false);
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

    }
}
