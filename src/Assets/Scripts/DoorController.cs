﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject door;

    [SerializeField]
    private float openSpeed = 5.0f;

    [SerializeField]
    private GameObject debrisParticle;

    [SerializeField]
    private float debrisStartDelay = 0.2f;

    private GameManager gameManager;
    private Utilities.ColorEnum crystalColor;
    private bool open = false;
    private Vector3 originalSize;
    private BoxCollider2D doorCollider;
    private Vector3 debrisPosition;
    private float debrisOffsetYMagicNumber = 0.25f;

    // Use this for initialization
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        crystalColor = GetComponent<CrystalController>().GetCrystalColor();
        doorCollider = door.GetComponent<BoxCollider2D>();

        originalSize = door.transform.localScale;

        var ps = debrisParticle.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startDelay = debrisStartDelay;

        var debrisOffsetY = doorCollider.bounds.extents.y - debrisOffsetYMagicNumber;
        debrisPosition = new Vector3(door.transform.position.x, door.transform.position.y + debrisOffsetY, door.transform.position.z);
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
            gameManager.PlayOpenDoorSound();
            open = true;
            doorCollider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            gameManager.PlayCloseDoorSound();
            open = false;
            doorCollider.enabled = true;
            Instantiate(debrisParticle, debrisPosition, debrisParticle.transform.rotation);
        }
    }
}
