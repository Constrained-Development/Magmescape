using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    [SerializeField]
    private GameObject lavaSplashParticle;
    [SerializeField]
    private float minSplashInterval = 0.25f;
    [SerializeField]
    private float maxSplashInterval = 0.5f;

    private float speed;
    private bool erupted;

    // Splash random position ranges (based on Lava tilemap local coordinates)
    private const float minSplashX = -8f;
    private const float maxSplashX = 8f;
    private const float splashY = -7.7f;

    private void Start()
    {
        StartCoroutine(LavaSplashes());
    }

    private void FixedUpdate()
    {
        if (transform.position.y >= Utilities.LAVA_STOP_POSITION)
        {
            speed = 0;
        }
        transform.position += new Vector3(0.0f, speed * Time.fixedDeltaTime, 0.0f);

        if (!erupted && transform.position.y >= Utilities.CAMERA_START_POSITION)
        {
            erupted = true;
        }
    }

    private IEnumerator LavaSplashes()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(minSplashInterval, maxSplashInterval));
            var pos = new Vector3(Random.Range(minSplashX, maxSplashX), splashY + transform.position.y, 0.0f);
            Instantiate(lavaSplashParticle, pos, lavaSplashParticle.transform.rotation);
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public bool IsErupted()
    {
        return erupted;
    }
}
