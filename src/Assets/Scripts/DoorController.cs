using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private float openSpeed = 5.0f;

    private Utilities.ColorEnum crystalColor;
    private bool open = false;
    private Vector3 originalSize;
    private BoxCollider2D doorCollider;

    // Use this for initialization
    private void Start()
    {
        crystalColor = GetComponent<CrystalController>().GetCrystalColor();
        doorCollider = door.GetComponent<BoxCollider2D>();


        originalSize = door.transform.localScale;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (open)
        {
            door.transform.localScale = Vector3.Lerp(door.transform.localScale, Vector3.zero, Time.fixedDeltaTime * openSpeed);
        }
        else
        {
            door.transform.localScale = Vector3.Lerp(door.transform.localScale, originalSize, Time.fixedDeltaTime * openSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            open = true;
            doorCollider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            open = false;
            doorCollider.enabled = true;
        }
    }
}
