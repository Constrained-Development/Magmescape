using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	// This field is hidden from the inspector because
	// it's value should only be set by the game manager
	[HideInInspector]
	public float Speed;

	private void FixedUpdate()
	{
		transform.position += new Vector3(0.0f, Speed * Time.fixedDeltaTime, 0.0f);
	}
}
