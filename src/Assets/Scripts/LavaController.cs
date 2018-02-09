using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    private float speed;
    private bool erupted;

    private void FixedUpdate()
    {
        transform.position += new Vector3(0.0f, speed * Time.fixedDeltaTime, 0.0f);

        if (!erupted && transform.position.y >= Utilities.GROUND_VERTICAL_POSITION)
        {
            erupted = true;
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
