using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
	[SerializeField]
	private float speed;

	private CameraController camera;

	private void Start()
	{
		camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}

	private void Update()
	{
		if (!camera.IsMoving && transform.position.y > GameManager.GROUND_VERTICAL_POSITION)
		{
			camera.StartMoving(speed);
		}
	}

	private void FixedUpdate()
	{
		transform.position += new Vector3(0.0f, speed * Time.fixedDeltaTime, 0.0f);
	}
}
