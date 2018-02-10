using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [SerializeField]
    private Utilities.ColorEnum gemColor;

    [SerializeField]
    private GameObject CollectParticle;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == gemColor)
        {
            Instantiate(CollectParticle, transform.position, CollectParticle.transform.rotation);
            gameManager.IncrementGems(gemColor);

            Destroy(gameObject);
        }
    }
}
