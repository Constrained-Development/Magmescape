using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Level" || other.tag == "MovingPlatform")
        {
            playerController.SetGrounded(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Level" || other.tag == "MovingPlatform")
        {
            playerController.SetGrounded(false);
        }
    }
}
