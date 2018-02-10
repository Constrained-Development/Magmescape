using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
	[SerializeField]
	private float speed;

	private Transform elevator;
	private Transform destination;
	private bool goingFromOrigin = true;
	private Vector3 origin;

	private void Start()
	{
		elevator = transform.Find("MovingPlatform");
		destination = transform.Find("Destination");
		origin = elevator.position;
	}

	private void FixedUpdate()
	{
		Vector3 dir;
		float move = Time.fixedDeltaTime * speed;

		if (goingFromOrigin)
		{
			dir = destination.position - elevator.position;

			if (dir.magnitude < move)
			{
				move = dir.magnitude;
				goingFromOrigin = false;
			}
		}
		else
		{
			dir = origin - elevator.position;

			if (dir.magnitude < move)
			{
				move = dir.magnitude;
				goingFromOrigin = true;
			}
		}

		elevator.position += (dir.normalized * move);
	}
}
