using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private CameraController cameraController;

    private void Start()
    {
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    private void Update()
    {
        if (!cameraController.IsMoving && transform.position.y > GameManager.GROUND_VERTICAL_POSITION)
        {
            cameraController.StartMoving(speed);
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0.0f, speed * Time.fixedDeltaTime, 0.0f);
    }
}
