﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed;

    private Vector3 realPosition;
    private Vector3 shakeOffset;

    private void Start()
    {
        realPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (transform.position.y >= Utilities.CAMERA_STOP_POSITION)
        {
            speed = 0;
        }
        realPosition += new Vector3(0.0f, speed * Time.fixedDeltaTime, 0.0f);
        transform.position = realPosition + shakeOffset;
    }

    public void Shake(float duration)
    {
        StartCoroutine("shake", duration);
    }

    private IEnumerator shake(float duration)
    {
        float timePassed = 0;
        while (timePassed < duration)
        {
            float randX = Random.Range(-5, 5) / 50f;
            float randY = Random.Range(-5, 5) / 50f;
            shakeOffset = new Vector3(randX, randY, 0);
            yield return new WaitForSecondsRealtime(0.05f);
            timePassed += 0.05f;
        }
        shakeOffset = Vector3.zero;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
