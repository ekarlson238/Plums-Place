using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaser : MonoBehaviour {

    private Vector3 startPosition;

    [SerializeField]
    private float moveLeftDistance;

    [SerializeField]
    private float speed;


	// Use this for initialization
	void Start () {
        startPosition = transform.position;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        transform.Translate(Vector3.left*speed);

        if(transform.position.x < (startPosition.x - moveLeftDistance))
        {
            speed = speed * -1;
        }
        if (transform.position.x > startPosition.x)
        {
            speed = speed * -1;
        }

    }
}
