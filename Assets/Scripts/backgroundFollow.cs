using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject thingToFollow;
    [SerializeField]
    private float yOffset;
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(thingToFollow.transform.position.x, thingToFollow.transform.position.y + yOffset, this.transform.position.z);
    }
}
