using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name == "Level_1")
            {
                Coin.LevelOneCoinCount = Coin.CoinCount;
            }
            if (SceneManager.GetActiveScene().name == "Level_2")
            {
                Coin.LevelTwoCoinCount = Coin.CoinCount;
            }
            if (SceneManager.GetActiveScene().name == "Level_3")
            {
                Coin.LevelThreeCoinCount = Coin.CoinCount;
            }
            Coin.CoinCount = 0;
            SceneManager.LoadScene(nextScene);
        }
    }
}
