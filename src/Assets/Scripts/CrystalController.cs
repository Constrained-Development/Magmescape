using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField]
    private Utilities.ColorEnum crystalColor;

    // Use this for initialization
    private void Start()
    {

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
            // Correct player entered
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            // Correct player exited
        }
    }

    public Utilities.ColorEnum GetCrystalColor()
    {
        return crystalColor;
    }
}
