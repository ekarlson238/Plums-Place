using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ifDestroyed : MonoBehaviour {

    [SerializeField]
    private GameObject self;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (self == null)
        {
            //reset scene

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
	}
}
