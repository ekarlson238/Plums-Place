using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This moves the laser back and forth between the start position and a point moveLeftDistance to the left of it.
/// The speed at which the laser moves at is set by the speed field.
/// </summary>
public class MovingLaser : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField]
    private float moveLeftDistance;
    [SerializeField]
    private float speed;

	// Use this for initialization
	private void Start ()
    {
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	private void FixedUpdate ()
    {
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
