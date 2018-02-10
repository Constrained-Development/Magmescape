using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private bool isOpen = false;
    private int playerCount = 0;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen)
        {
            return;
        }

        if (other.tag == "Player")
        {
            playerCount++;
            if (playerCount == 2)
            {
                isOpen = true;
                animator.SetLayerWeight(1, 1.0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerCount--;
        }
    }

}
