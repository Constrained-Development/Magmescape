using System.Collections;
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
            float randX = Random.Range(-5, 5) / 100f;
            float randY = Random.Range(-5, 5) / 100f;
            shakeOffset = new Vector3(randX, randY, 0);
            yield return new WaitForSecondsRealtime(0.1f);
            timePassed += 0.1f;
        }
        shakeOffset = Vector3.zero;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
