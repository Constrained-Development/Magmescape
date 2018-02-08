using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private float speed;
	private bool isMoving;

	public bool IsMoving { get { return isMoving; } }

	public void StartMoving(float speed)
	{
		this.speed = speed;
		this.isMoving = true;
	}

	private void FixedUpdate()
	{
		if (isMoving)
		{
			transform.position += new Vector3(0.0f, speed * Time.fixedDeltaTime, 0.0f);
		}
	}
}
