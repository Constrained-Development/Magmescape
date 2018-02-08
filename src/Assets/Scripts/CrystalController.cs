using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField]
    private Utilities.ColorEnum crystalColor;

    private Animator animator;

    // Use this for initialization
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            animator.SetLayerWeight(1, 1.0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            animator.SetLayerWeight(1, 0.0f);
        }
    }

    public Utilities.ColorEnum GetCrystalColor()
    {
        return crystalColor;
    }
}
