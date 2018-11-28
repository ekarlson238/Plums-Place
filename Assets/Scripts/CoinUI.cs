using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    [SerializeField]
    private Text coinText;
    private const string labelPrefix = "Coins: ";
    private const string outOfTotal = "/3";

    public void UpdateScoreText()
    {
        coinText.text = labelPrefix + Coin.CoinCount + outOfTotal;
    }
}
