using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float lavaSpeed = 1.0f;

    private Transform lava;

    // Use this for initialization
    private void Start()
    {
        lava = GameObject.FindGameObjectWithTag("Lava").transform;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        lava.position += new Vector3(0.0f, lavaSpeed * Time.fixedDeltaTime, 0.0f);
    }
}
