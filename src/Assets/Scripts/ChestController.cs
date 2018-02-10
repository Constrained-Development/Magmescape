using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private bool isOpen = false;
    private int playerCount = 0;

    private GameManager gameManager;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                playerCount = 0;
                animator.SetLayerWeight(1, 1.0f);
                gameManager.WinGame();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isOpen)
        {
            return;
        }

        if (other.tag == "Player")
        {
            playerCount--;
        }
    }

}
