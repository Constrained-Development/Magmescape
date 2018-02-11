using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField]
    private Utilities.ColorEnum crystalColor;

    [SerializeField]
    private GameObject activatedParticle;

    private GameManager gameManager;
    private Animator animator;
    private ParticleSystem.EmissionModule activatedParticleEmission;

    // Use this for initialization
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();

        var activatedParticleInstance = Instantiate(activatedParticle, transform.position, activatedParticle.transform.rotation);
        var activatedParticleSystem = activatedParticleInstance.GetComponent<ParticleSystem>();
        activatedParticleEmission = activatedParticleSystem.emission;
        activatedParticleEmission.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            gameManager.PlayActivateCrystalSound();
            activatedParticleEmission.enabled = true;
            animator.SetLayerWeight(1, 1.0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" &&
            other.gameObject.GetComponent<PlayerController>().GetPlayerColor() == crystalColor)
        {
            gameManager.PlayDeactivateCrystalSound();
            activatedParticleEmission.enabled = false;
            animator.SetLayerWeight(1, 0.0f);
        }
    }

    public Utilities.ColorEnum GetCrystalColor()
    {
        return crystalColor;
    }
}
