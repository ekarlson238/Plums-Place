using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject tutorialText;

    /// <summary>
    /// Shows the tutorial UI text when the player enters the trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            tutorialText.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the tutorial UI text when the player exits the trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            tutorialText.SetActive(false);
        }
    }
}
