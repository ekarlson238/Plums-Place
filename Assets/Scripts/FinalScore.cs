using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour {

    private string lvl1Prefix = "Level 1:  ";
    private string lvl2Prefix = "Level 2:  ";
    private string lvl3Prefix = "Level 3:  ";

    [SerializeField]
    private Text lvl1Text;
    [SerializeField]
    private Text lvl2Text;
    [SerializeField]
    private Text lvl3Text;

    void Start () {
        lvl1Text.text = lvl1Prefix + Coin.levelOneCoinCount;
        lvl2Text.text = lvl2Prefix + Coin.levelTwoCoinCount;
        lvl3Text.text = lvl3Prefix + Coin.levelThreeCoinCount;
    }
	
}
