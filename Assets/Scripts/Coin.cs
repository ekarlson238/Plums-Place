﻿using System.Collections;
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

    #region Levels Coins
    public static int LevelOneCoinCount;
    public static int LevelTwoCoinCount;
    public static int LevelThreeCoinCount;
    #endregion

    private void Awake()
    {
        coinUI = GameObject.Find("CoinsCollectedUI").GetComponent<CoinUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isCollected)
            {
                isCollected = true;
                CoinCount++;
                coinUI.UpdateScoreText();
            }
            gameObject.SetActive(false);
        }
    }
}
