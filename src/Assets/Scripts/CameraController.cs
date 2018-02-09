using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed;

    private void FixedUpdate()
    {
        transform.position += new Vector3(0.0f, speed * Time.fixedDeltaTime, 0.0f);
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
