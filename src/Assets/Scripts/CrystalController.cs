using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField]
    private Utilities.ColorEnum crystalColor;

    private GameManager gameManager;
    private Animator animator;

    // Use this for initialization
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            gameManager.PlayActivateCrystalSound();
            animator.SetLayerWeight(1, 1.0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            gameManager.PlayDeactivateCrystalSound();
            animator.SetLayerWeight(1, 0.0f);
        }
    }

    public Utilities.ColorEnum GetCrystalColor()
    {
        return crystalColor;
    }
}
