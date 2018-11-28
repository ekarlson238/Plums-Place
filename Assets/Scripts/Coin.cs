using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static int coinCount_UseProperty;
    private CoinUI coinUI;
    private bool isCollected;
    public static int CoinCount
    {
        set
        {
            if (value >= 0)
                coinCount_UseProperty = value;
        }
        get
        {
            return coinCount_UseProperty;
        }
    }

    private void Awake()
    {
        coinUI = GameObject.Find("CoinsCollectedUI").GetComponent<CoinUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //if touches coin
        {
            if (!isCollected)
            {
                isCollected = true;
                CoinCount++;
                coinUI.UpdateScoreText();
            }
            //Leaving this here so I know what I tried when I try to fix the coin collection bug again
            /*
            Destroy(collision.gameObject); //destroy the coin
            coinsCollected += 1; //add one to coins collected
            coinText.text = "Coins: " + coinsCollected + "/3";
            */
            /*
            if(collision.gameObject != null)
            {
                coinsCollected += 1; //add one to coins collected
                coinText.text = "Coins: " + coinsCollected + "/3";
                Destroy(collision.gameObject); //destroy the coin
            }
            */
            gameObject.SetActive(false);
            //coinsCollected += 1; //add one to coins collected
            //coinText.text = "Coins: " + coinsCollected + "/3";
        }
    }
}
