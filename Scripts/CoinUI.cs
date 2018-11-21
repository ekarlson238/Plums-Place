using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour {

    [SerializeField]
    private Text coinText;

    public void UpdateScoreText()
    {
        coinText.text = "Coins: " + Coin.CoinCount + "/3";
    }
}
